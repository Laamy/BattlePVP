using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static User32;

class BattlefieldClient
{
    // information about battlefield like its process ID

    /// <summary>
    /// Unsigned int (To store the battlefield handle in for winhooks)
    /// </summary>
    public static uint GameId;

    /// <summary>
    /// Int Pointer (32bit number) (To store the battlefiel window handle in for winhooks, again)
    /// </summary>
    public static IntPtr WinHandle;

    /// <summary>
    /// Name of the game
    /// </summary>
    public static string GameName = "BF2042";
    public static string GameTitle = "Battlefield™ 2042";

    public static void OpenGame()
    {
        try
        {
            // locate rust in the memory
            Process game = Process.GetProcessesByName(GameName)[0]; // oops title is Battlefield™ 2042

            GameId = (uint)game.Id; // game id
            WinHandle = game.MainWindowHandle; // window handle
        }
        catch
        {
            Console.WriteLine("Please open the game first..");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            Process.GetCurrentProcess().Kill();
        }
    }

    public static ProcessRectangle GetGameRect()
    {
        // get window rect into "rect" variable
        ProcessRectangle rect;
        User32.GetWindowRect(WinHandle, out rect);

        // return window dimensions
        return rect;
    }

    public static bool isGameFocused()
    {
        var sb = new StringBuilder(GameTitle.Length + 1);
        GetWindowText(GetForegroundWindow(), sb, GameTitle.Length + 1);
        return sb.ToString().CompareTo(GameTitle) == 0;
    }

    public static IntPtr IsGameFocusedInsert()
    {
        var sb = new StringBuilder(GameTitle.Length + 1);
        GetWindowText(GetForegroundWindow(), sb, GameTitle.Length + 1);
        if (sb.ToString() == GameTitle)
            return (IntPtr)(-1);
        return (IntPtr)(-2);
    }

    #region Structs
    /// <summary>
    /// Window Rectangle
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessRectangle
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public ProcessRectangle(Point position, Point size) // this is most likely wrong
        {
            this.Left = position.X;
            this.Top = position.X + size.X;
            this.Right = position.Y;
            this.Bottom = position.Y + size.Y;

            // Left, Top, Right, Bottom
            // X, X - X, Y, Y - Y

            // Left, Top,
            // Right, Bottom
            // X, X - X,
            // Y, Y - Y
        }
    }
    #endregion
}