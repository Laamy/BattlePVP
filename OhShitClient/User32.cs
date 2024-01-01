using static Overlay;
using static BattlefieldClient;

using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Windows.Forms;

[StructLayout(LayoutKind.Sequential)]
public struct CURSORINFO
{
    public Int32 cbSize;
    public Int32 flags;
    public IntPtr hCursor;
    public POINTAPI ptScreenPos;
}

[StructLayout(LayoutKind.Sequential)]
public struct POINTAPI
{
    public int x;
    public int y;
}

class User32
{
    [DllImport("User32.dll")]
    public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

    [DllImport("User32.dll")]
    public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

    [DllImport("User32.dll")]
    public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
        WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    [DllImport("User32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr voidProcessId);

    [DllImport("User32.dll", SetLastError = true)]
    public static extern bool GetWindowRect(IntPtr hWnd, out ProcessRectangle lpRect);

    [DllImport("User32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("User32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    public static extern bool GetAsyncKeyState(char v);

    [DllImport("User32.dll")]
    public static extern bool GetCursorInfo(out CURSORINFO pci);

    [DllImport("User32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("User32.dll")]
    public static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("User32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("User32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
}