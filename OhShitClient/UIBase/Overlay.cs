using System;
using System.Windows.Forms;
using System.Drawing;

using static BattlefieldClient;
using static User32;
using static Debug;

class Overlay : Form
{
    public static Overlay handle;

    public Overlay()
    {
        Log("Initializing  UI..");

        InitializeComponents();

        handle = this;

        Log("Initializing  Paint Hooks..");
        Paint += OnUpdate;

        Log("Initializing  Delegate..");

        Focus(); // bring the overlay window into focus (avoids it showing up behind the game)

        WinGraphics.SetClickthrough(this.Handle); // allows the user to click through the overlay even if its currently being drawn to
        WinGraphics.AddAdjust(this.Handle, OnAdjust); // Setup the adjust delegate for hooking

        Log("Initializing  WinHooks..");

        WinGraphics.InitDelegate(this.Handle); // hook the window event delegate
    }

    public void OnUpdate(object sender, PaintEventArgs e)
    {
        // lets check if the cursor is visible if so then we dont draw a crosshair
        if (Keymap.IsCursorVisible() == true) // || Keymap.GetDown(Keys.Tab) later ig
            return;

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