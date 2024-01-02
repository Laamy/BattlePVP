using System;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

using Color = SharpDX.Color;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

using static User32;
using static Debug;

class Overlay : Form
{
    public static Overlay handle;

    public RenderContext context;
    //private BitmapRenderTarget backTarget;

    public static string command = "";

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

        context = new RenderContext(new WindowRenderTarget(factory, renderTargetProperties, properties)
        {
            TextAntialiasMode = TextAntialiasMode.Aliased,
            AntialiasMode = AntialiasMode.Aliased
        })
        {
            clearColour = Color.Magenta
        };

        //backTarget = new BitmapRenderTarget(target, CompatibleRenderTargetOptions.None);
    }

    public void InitializeComponents()
    {
        // initialize winform here
        TopMost = true; // not needed
        TransparencyKey = System.Drawing.Color.Magenta;
        //Opacity = 1;

        FormBorderStyle = FormBorderStyle.None;

        Text = BattlefieldClient.GameTitle;
    }

    public void OnAdjust(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        // get the rough dimensions of the window
        RectangleF rect = BattlefieldClient.WindowDims;

        // set the window where the battlefield game is (while also ontop)
        SetWindowPos(Handle, BattlefieldClient.isFocusedInsert, (int)rect.Location.X, (int)rect.Location.Y, (int)rect.Size.Width, (int)rect.Size.Height, 0x40);
    }

    private void OnUpdate() // OnUpdate
    {
        context.Begin();
        context.Clear(context.clearColour);

        // lets check if the cursor is visible if so then we dont draw a crosshair
        if (BattlefieldClient.CanUseMoveKeys && !BattlefieldClient.Keymap.GetDown(Keys.Tab)) // || Keymap.GetDown(Keys.Tab) later ig
        {
            // lets quickly draw a test crosshair
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;

            Color4 green = new Color4(0, 1, 0, 1);

            context.DrawLine(new Vector2(centerX - 4, centerY), new Vector2(centerX + 4, centerY), green, 2);
            context.DrawLine(new Vector2(centerX, centerY - 4), new Vector2(centerX, centerY + 4), green, 2);
        }

        {
        if (Program.CmdBar)
            // console title
            context.FillRectangle(new Vector2(30,30), new Vector2(150, 18), Color4.White);

            // shadow'd string
            context.DrawString("Console", new Vector2(39, 30), new Color4(0.3f, 0.3f, 0.3f, 1), "System", 12);
            context.DrawString("Console", new Vector2(40, 31), Color.Black, "System", 12);

            context.FillRectangle(new Vector2(30, 48), new Vector2(970, 52), new Color4(0.3f, 0.3f, 0.3f, 1));

            context.DrawString("> " + command, new Vector2(39, 54), Color.Black, "System", 24);
            context.DrawString("> " + command, new Vector2(40, 55), Color.White, "System", 24);
        }

        //context.DrawString("DEBUG MODE ACTIVE", new Vector2(10, 10), new Color4(1, 0, 0, 1), "Arial", 24);

        context.End();
    }

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

        context?.target.Resize(new Size2(ClientSize.Width, ClientSize.Height));
    }

    protected override void Dispose(bool disposing)
    {
        context?.target.Dispose();

        base.Dispose(disposing);
    }
}