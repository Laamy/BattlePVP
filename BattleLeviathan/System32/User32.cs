using System.Runtime.InteropServices;
using System;
using System.Text;

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

[StructLayout(LayoutKind.Sequential)]
struct INPUT
{
    public uint type;
    public InputUnion u;

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;

        [FieldOffset(0)]
        public KEYBDINPUT ki;

        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }
}

[StructLayout(LayoutKind.Sequential)]
struct MOUSEINPUT
{
    public int dx;
    public int dy;
    public uint mouseData;
    public uint dwFlags;
    public uint time;
    public IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
struct KEYBDINPUT
{
    public ushort wVk;
    public ushort wScan;
    public uint dwFlags;
    public uint time;
    public IntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
struct HARDWAREINPUT
{
    public uint uMsg;
    public ushort wParamL;
    public ushort wParamH;
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

    [DllImport("User32.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

    [DllImport("User32.dll", SetLastError = true)]
    public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
}