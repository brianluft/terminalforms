using System.Diagnostics;
using System.Runtime.InteropServices;

// Terminal size for tests (must match Application.cpp debug screenshot dimensions)
const int Rows = 12;
const int Cols = 40;
const int TimeoutMs = 30000;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: PtyRunner <command> [args...]");
    return 1;
}

var fileName = args[0];
var arguments = string.Join(" ", args.Skip(1).Select(a => a.Contains(' ') ? $"\"{a}\"" : a));

return PtyProcess.Run(fileName, arguments, Rows, Cols, TimeoutMs);

/// <summary>
/// Runs a process inside a pseudo-terminal (PTY) on Linux.
/// </summary>
static class PtyProcess
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

    public static int Run(string fileName, string arguments, int rows, int cols, int timeoutMs)
    {
        int masterFd = posix_openpt(O_RDWR | O_NOCTTY);
        if (masterFd == -1)
            throw new InvalidOperationException(
                $"posix_openpt failed: {Marshal.GetLastWin32Error()}"
            );

        try
        {
            if (grantpt(masterFd) == -1)
                throw new InvalidOperationException(
                    $"grantpt failed: {Marshal.GetLastWin32Error()}"
                );

            if (unlockpt(masterFd) == -1)
                throw new InvalidOperationException(
                    $"unlockpt failed: {Marshal.GetLastWin32Error()}"
                );

            IntPtr slaveNamePtr = ptsname(masterFd);
            if (slaveNamePtr == IntPtr.Zero)
                throw new InvalidOperationException(
                    $"ptsname failed: {Marshal.GetLastWin32Error()}"
                );

            string slavePath = Marshal.PtrToStringAnsi(slaveNamePtr)!;

            int slaveFd = open(slavePath, O_RDWR);
            if (slaveFd == -1)
                throw new InvalidOperationException(
                    $"open slave failed: {Marshal.GetLastWin32Error()}"
                );

            try
            {
                var winSize = new WinSize { ws_row = (ushort)rows, ws_col = (ushort)cols };
                ioctl(slaveFd, TIOCSWINSZ, ref winSize);
            }
            finally
            {
                close(slaveFd);
            }

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
            process.StandardInput.Close();

            var cts = new CancellationTokenSource();
            var drainTask = Task.Run(() =>
            {
                var buffer = new byte[4096];
                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        if (read(masterFd, buffer, buffer.Length) <= 0)
                            break;
                    }
                }
                catch { }
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
