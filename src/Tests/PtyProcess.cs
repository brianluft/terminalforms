using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Tests;

/// <summary>
/// Runs a process inside a pseudo-terminal (PTY) on Linux.
/// This is necessary because tvision requires a real terminal to initialize the screen buffer.
/// </summary>
internal static class PtyProcess
{
    private const int TIOCSWINSZ = 0x5414;
    private const int O_RDWR = 2;
    private const int O_NOCTTY = 256;

    [StructLayout(LayoutKind.Sequential)]
    private struct WinSize
    {
        public ushort ws_row;
        public ushort ws_col;
        public ushort ws_xpixel;
        public ushort ws_ypixel;
    }

    [DllImport("libc", SetLastError = true)]
    private static extern int posix_openpt(int flags);

    [DllImport("libc", SetLastError = true)]
    private static extern int grantpt(int fd);

    [DllImport("libc", SetLastError = true)]
    private static extern int unlockpt(int fd);

    [DllImport("libc", SetLastError = true)]
    private static extern IntPtr ptsname(int fd);

    [DllImport("libc", SetLastError = true)]
    private static extern int open(string path, int flags);

    [DllImport("libc", SetLastError = true)]
    private static extern int close(int fd);

    [DllImport("libc", SetLastError = true)]
    private static extern int ioctl(int fd, ulong request, ref WinSize winp);

    [DllImport("libc", SetLastError = true)]
    private static extern nint read(int fd, byte[] buf, nint count);

    /// <summary>
    /// Runs a process in a PTY with the specified terminal size.
    /// </summary>
    /// <param name="fileName">The executable to run.</param>
    /// <param name="arguments">Command line arguments.</param>
    /// <param name="rows">Terminal rows (height).</param>
    /// <param name="cols">Terminal columns (width).</param>
    /// <param name="timeoutMs">Timeout in milliseconds.</param>
    /// <returns>The process exit code.</returns>
    public static int Run(string fileName, string arguments, int rows, int cols, int timeoutMs)
    {
        // Create master side of PTY
        int masterFd = posix_openpt(O_RDWR | O_NOCTTY);
        if (masterFd == -1)
        {
            throw new InvalidOperationException(
                $"posix_openpt failed with errno {Marshal.GetLastWin32Error()}"
            );
        }

        try
        {
            // Grant access to slave
            if (grantpt(masterFd) == -1)
            {
                throw new InvalidOperationException(
                    $"grantpt failed with errno {Marshal.GetLastWin32Error()}"
                );
            }

            // Unlock slave
            if (unlockpt(masterFd) == -1)
            {
                throw new InvalidOperationException(
                    $"unlockpt failed with errno {Marshal.GetLastWin32Error()}"
                );
            }

            // Get slave device path
            IntPtr slaveNamePtr = ptsname(masterFd);
            if (slaveNamePtr == IntPtr.Zero)
            {
                throw new InvalidOperationException(
                    $"ptsname failed with errno {Marshal.GetLastWin32Error()}"
                );
            }
            string slavePath = Marshal.PtrToStringAnsi(slaveNamePtr)!;

            // Open slave to set terminal size
            int slaveFd = open(slavePath, O_RDWR);
            if (slaveFd == -1)
            {
                throw new InvalidOperationException(
                    $"open slave failed with errno {Marshal.GetLastWin32Error()}"
                );
            }

            try
            {
                // Set terminal size on slave
                var winSize = new WinSize
                {
                    ws_row = (ushort)rows,
                    ws_col = (ushort)cols,
                    ws_xpixel = 0,
                    ws_ypixel = 0,
                };
                ioctl(slaveFd, TIOCSWINSZ, ref winSize);
            }
            finally
            {
                close(slaveFd);
            }

            // Build a shell command that redirects the child's stdin/stdout/stderr to the PTY slave
            var shellCommand = $"{fileName} {arguments} <'{slavePath}' >'{slavePath}' 2>&1";

            var psi = new ProcessStartInfo("/bin/sh")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            psi.ArgumentList.Add("-c");
            psi.ArgumentList.Add(shellCommand);
            psi.Environment["TERM"] = "xterm-256color";
            psi.Environment["COLUMNS"] = cols.ToString();
            psi.Environment["LINES"] = rows.ToString();

            using var process = Process.Start(psi)!;

            // Close stdin immediately
            process.StandardInput.Close();

            // Drain the master fd to prevent the child from blocking on writes
            var cts = new CancellationTokenSource();
            var drainTask = Task.Run(() =>
            {
                var buffer = new byte[4096];
                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        nint bytesRead = read(masterFd, buffer, buffer.Length);
                        if (bytesRead <= 0)
                            break;
                    }
                }
                catch
                {
                    // Ignore read errors during drain
                }
            });

            if (!process.WaitForExit(timeoutMs))
            {
                process.Kill();
                cts.Cancel();
                throw new TimeoutException($"Process timed out after {timeoutMs}ms");
            }

            cts.Cancel();
            return process.ExitCode;
        }
        finally
        {
            close(masterFd);
        }
    }
}
