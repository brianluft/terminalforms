namespace TerminalForms;

/// <summary>
/// Represents a size in 2D space.
/// </summary>
/// <param name="Width">The width of the size.</param>
/// <param name="Height">The height of the size.</param>
[StructLayout(LayoutKind.Sequential)]
public record struct Size(int Width, int Height);
