using System;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

class DirectXOverlay : Form
{
    private WindowRenderTarget target;

    private SolidColorBrush testBrush;

    public DirectXOverlay()
    {
        TransparencyKey = System.Drawing.Color.White;

        InitializeDevice();
        OnUpdate();
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

        testBrush = new SolidColorBrush(target, Color4.Black);
    }

    private void OnUpdate() // OnUpdate
    {
        target.BeginDraw();
        target.Clear(Color.White);

        target.DrawLine(new Vector2(10, 10), new Vector2(100, 100), testBrush);

        var textFormat = new TextFormat(new SharpDX.DirectWrite.Factory(), "Arial", 12);
        target.DrawText("Hello, DirectX!", textFormat, new RectangleF(10, 120, 2000, 2000), testBrush);

        target.EndDraw();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        OnUpdate();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        target.Resize(new Size2(ClientSize.Width, ClientSize.Height));
    }

    protected override void Dispose(bool disposing)
    {
        testBrush.Dispose();
        target.Dispose();

        base.Dispose(disposing);
    }
}