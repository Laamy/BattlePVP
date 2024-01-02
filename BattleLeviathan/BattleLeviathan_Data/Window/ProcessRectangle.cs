using System.Drawing;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct ProcessRectangle
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
    public ProcessRectangle(Point position, Point size)
    {
        this.Left = position.X;
        this.Top = position.X + size.X;
        this.Right = position.Y;
        this.Bottom = position.Y + size.Y;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        ProcessRectangle a2 = (ProcessRectangle)obj;

        return Left == a2.Left && Top == a2.Top && Right == a2.Right && Bottom == a2.Bottom;
    }
}