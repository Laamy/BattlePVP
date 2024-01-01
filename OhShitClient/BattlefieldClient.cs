using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static User32;

/// <summary>
/// Information about battlefield like its process ID
/// </summary>
class BattlefieldClient
{
    /// <summary>
    /// Unsigned int (To store the battlefield handle in for winhooks)
    /// </summary>
    public static uint GameId;

    /// <summary>
    /// Int Pointer (32bit number) (To store the battlefiel window handle in for winhooks, again)
    /// </summary>
    public static IntPtr WinHandle;

    /// <summary>
    /// Executable name of the game
    /// </summary>
    public static string GameName = "BF2042";

    /// <summary>
    /// Title name of the game
    /// </summary>
    public static string GameTitle = "Battlefield™ 2042";

    /// <summary>
    /// Get information about the game keymap & events
    /// </summary>
    public static Keymap Keymap = new Keymap();

    /// <summary>
    /// Variable for if the game is focused or not
    /// </summary>
    public static bool isFocused
    {
        get
        {
            CreateInstance(); // check create

            var sb = new StringBuilder(GameTitle.Length + 1);
            GetWindowText(GetForegroundWindow(), sb, GameTitle.Length + 1);
            return sb.ToString().CompareTo(GameTitle) == 0;
        }
    }

    /// <summary>
    /// Variable for if the game is focused or not insert
    /// </summary>
    public static IntPtr isFocusedInsert
    {
        get
        {
            CreateInstance(); // check create

            if (isFocused)
                return (IntPtr)(-1);
            return (IntPtr)(-2);
        }
    }

    /// <summary>
    /// Window dimensions
    /// </summary>
    public static ProcessRectangle WindowDims
    {
        get
        {
            CreateInstance(); // check create

            ProcessRectangle rect;
            GetWindowRect(WinHandle, out rect);

            return rect;
        }
    }

    /// <summary>
    /// If the user is able to use the movement keys or not
    /// </summary>
    /// <returns></returns>
    public static bool CanUseMoveKeys
    {
        get
        {
            CURSORINFO cursorInfo = new CURSORINFO();
            cursorInfo.cbSize = Marshal.SizeOf(typeof(CURSORINFO));

            if (User32.GetCursorInfo(out cursorInfo))
                return (cursorInfo.flags & 0x00000001) != 0;

            return true;
        }
    }

    /// <summary>
    /// doesnt work rn
    /// </summary>
    /// <param name="command"></param>
    public static void SendCommand(string command)
    {
        if (isFocused)
        {
            Keymap.SimulatePress(192);
            Keymap.SimulateRelease(192);

            //SendKeys.SendWait(command);
            //SendKeys.SendWait(((char)Keys.Oemtilde).ToString());
        }
        else Console.WriteLine("Need to be in focus to send game commands");
    }

    /// <summary>
    /// Locate the game and setup the game interface instance
    /// </summary>
    public static void CreateInstance()
    {
        if (GameId != 0)
            return;

        try
        {
            // locate battlefield in the memory
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
}