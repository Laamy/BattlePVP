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

        Focus(); // bring the overlay window into focus (avoids it showing up behind the game)

        int initStyle = GetWindowLong(Handle, -20);
        SetWindowLong(Handle, -20, initStyle | 0x80000 | 0x20); // allows the user to click through the overlay even if its currently being drawn to

        overDel = new WinEventDelegate(OnAdjust);

        Log("Initializing  WinHooks..");

        // hook OnAdjust window event delegare (cross thread event stuff for the window)
        // hooks : WindowMoved, WindowClicked
        SetWinEventHook((uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, (uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, overDel, GameId, GetWindowThreadProcessId(WinHandle, IntPtr.Zero), (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
        SetWinEventHook((uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, (uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, overDel, 0, 0, (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
    }

    public void OnUpdate(object sender, PaintEventArgs e)
    {
        // lets quickly draw a test crosshair

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
        // get the rough dimensions of the window
        ProcessRectangle rect = GetGameRect();

        // Doesnt support maximized window
        int x = rect.Left; // this should cover when your dragging the window around and alt + enter (f11) fullscreen
        int y = rect.Top;
        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;

        // set the window where the battlefield game is (while also ontop)
        SetWindowPos(Handle, IsGameFocusedInsert(), x, y, width, height, 0x40);
    }
}