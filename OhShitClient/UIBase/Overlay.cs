using System;
using System.Windows.Forms;
using System.Drawing;

using static BattlefieldClient;
using static User32;
using static Debug;

class Overlay : Form
{
    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    public static WinEventDelegate overDel;

    public Overlay()
    {
        Log("Initializing  UI..");

        InitializeComponents();

        Log("Initializing  Paint Hooks..");
        Paint += OnUpdate;

        Log("Initializing  Delegate..");

        Focus(); // bring the overlay window into focus

        // click through
        int initStyle = GetWindowLong(Handle, -20);
        SetWindowLong(Handle, -20, initStyle | 0x80000 | 0x20);

        overDel = new WinEventDelegate(OnAdjust);

        Log("Initializing  WinHooks..");

        SetWinEventHook((uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, (uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, overDel, GameId, GetWindowThreadProcessId(WinHandle, IntPtr.Zero), (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
        SetWinEventHook((uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, (uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, overDel, 0, 0, (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
    }

    public void OnUpdate(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        int centerX = e.ClipRectangle.Width / 2;
        int centerY = e.ClipRectangle.Height / 2;

        Pen pen = new Pen(Color.Green, 2);

        g.DrawLine(pen, centerX - 4, centerY, centerX + 4, centerY);

        g.DrawLine(pen, centerX, centerY - 4, centerX, centerY + 4);
    }

    public void InitializeComponents()
    {
        // initialize winform here
        TopMost = true; // not needed
        TransparencyKey = BackColor;

        FormBorderStyle = FormBorderStyle.None;

        Text = GameTitle;

        DoubleBuffered = true; // no tearing cuz this is gay
    }

    public void OnAdjust(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        ProcessRectangle rect = GetGameRect();

        int x = rect.Left; // wont do fullscreen yet
        int y = rect.Top;
        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;

        SetWindowPos(Handle, IsGameFocusedInsert(), x, y, width, height, 0x40); // might be working
    }
}