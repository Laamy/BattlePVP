using System;
using System.Windows.Forms;
using System.Drawing;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

using static User32;
using static Debug;
using Color = SharpDX.Color;

class Overlay : Form
{
    public static Overlay handle;

    private WindowRenderTarget target;
    //private BitmapRenderTarget backTarget;

    public Overlay()
    {
        Log("Initializing Overlay UI..");

        InitializeComponents();

        handle = this;

        Log("Initializing Overlay DirectX..");

        InitializeDevice();
        SetStyle(ControlStyles.OptimizedDoubleBuffer, false); // breaks directx (idky tbh)
        OnUpdate();

        Log("Initializing Overlay WinHooks..");

        Focus(); // bring the overlay window into focus (avoids it showing up behind the game)

        WinGraphics.SetClickthrough(this.Handle); // allows the user to click through the overlay even if its currently being drawn to

        WinGraphics.AddAdjust(this.Handle, OnAdjust); // Setup the adjust delegate for hooking
        WinGraphics.InitDelegate(this.Handle); // hook the window event via delegate
    }

    public SolidColorBrush GetBrush(Color4 colour)
    {
        return new SolidColorBrush(target, colour);
    }

    private void InitializeDevice()
    {
        var factory = new Factory();
        var properties = new HwndRenderTargetProperties
        {
            Hwnd = Handle,
            PixelSize = new Size2(ClientSize.Width, ClientSize.Height),
            PresentOptions = PresentOptions.Immediately
        };

        var renderTargetProperties = new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied));

        target = new WindowRenderTarget(factory, renderTargetProperties, properties)
        {
            TextAntialiasMode = TextAntialiasMode.Aliased,
            AntialiasMode = AntialiasMode.Aliased
        };

        //backTarget = new BitmapRenderTarget(target, CompatibleRenderTargetOptions.None);
    }

    public void InitializeComponents()
    {
        // initialize winform here
        TopMost = true; // not needed
        TransparencyKey = System.Drawing.Color.White;

        FormBorderStyle = FormBorderStyle.None;

        Text = BattlefieldClient.GameTitle;
    }

    public void OnAdjust(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        // get the rough dimensions of the window
        ProcessRectangle rect = BattlefieldClient.WindowDims;

        // Doesnt support maximized window
        int x = rect.Left; // this should cover when your dragging the window around and alt + enter (f11) fullscreen
        int y = rect.Top;
        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;

        // set the window where the battlefield game is (while also ontop)
        SetWindowPos(Handle, BattlefieldClient.isFocusedInsert, x, y, width, height, 0x40);
    }

    private void OnUpdate() // OnUpdate
    {
        target.BeginDraw();
        target.Clear(Color.White);

        // lets check if the cursor is visible if so then we dont draw a crosshair
        if (BattlefieldClient.CanUseMoveKeys && !Keymap.GetDown(Keys.Tab)) // || Keymap.GetDown(Keys.Tab) later ig
        {
            // lets quickly draw a test crosshair
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;

            SolidColorBrush green = GetBrush(new Color4(0, 255, 0, 255));

            target.DrawLine(new Vector2(centerX - 4, centerY), new Vector2(centerX + 4, centerY), green, 2);
            target.DrawLine(new Vector2(centerX, centerY - 4), new Vector2(centerX, centerY + 4), green, 2);

            green.Dispose();
        }

        //target.DrawLine(new Vector2(0,0), new Vector2(ClientSize.Width, ClientSize.Height), testBrush);

        //var textFormat = new TextFormat(new SharpDX.DirectWrite.Factory(), "Arial", 12);
        //target.DrawText("Hello, DirectX!", textFormat, new RectangleF(10, 120, 2000, 2000), testBrush);

        target.EndDraw();

        // present frame to screen
        //Present(target, target);
    }

    //public void OnUpdate(object sender, PaintEventArgs e)
    //{

    //    Graphics g = e.Graphics;

    //    //g.FillRectangle(new SolidBrush(Color.FromArgb(1, 255, 0, 0)), new RectangleF(new Point(0, 0), new SizeF(60, 60)));

    //    // lets check if the cursor is visible if so then we dont draw a crosshair
    //    if (BattlefieldClient.CanUseMoveKeys == true) // || Keymap.GetDown(Keys.Tab) later ig
    //        return;

    //    // lets quickly draw a test crosshair
    //    int centerX = e.ClipRectangle.Width / 2;
    //    int centerY = e.ClipRectangle.Height / 2;

    //    Pen pen = new Pen(Color.Green, 2);

    //    g.DrawLine(pen, centerX - 4, centerY, centerX + 4, centerY);
    //    g.DrawLine(pen, centerX, centerY - 4, centerX, centerY + 4);
    //}

    protected override void OnPaint(PaintEventArgs e)
    {
        OnUpdate();

        base.OnPaint(e);
    }

    protected override void OnPaintBackground(PaintEventArgs e) // this caused me so much pain and i just realized it now
    {
        // DO NOT DELETE THIS BIT OF CODE! its cancelling the background being redrawn to white!
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        target?.Resize(new Size2(ClientSize.Width, ClientSize.Height));
    }

    protected override void Dispose(bool disposing)
    {
        target?.Dispose();

        base.Dispose(disposing);
    }
}