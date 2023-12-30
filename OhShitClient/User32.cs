using static Overlay;
using System.Runtime.InteropServices;
using System;
using static BattlefieldClient;
using System.Text;

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
    
    [DllImport("User32.dll")]
    public static extern IntPtr GetForegroundWindow();
}