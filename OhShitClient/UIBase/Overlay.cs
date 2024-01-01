using System;
using System.Windows.Forms;
using System.Drawing;

using OhShitClient.UIBase;

using static BattlefieldClient;
using static User32;
using static Debug;

using SDL2;
using System.Drawing.Imaging;

class Overlay : Form
{
    public static Overlay handle;
    public static bool g_isRendering = false;

    //public IntPtr renderer;
    //public IntPtr window;
    //public bool g_IsRunning = true;
    //public PictureBox display;

    public Overlay()
    {
        Log("Initializing Overlay UI..");

        InitializeComponents();

        TransparencyKey = Color.FromArgb(255, 255, 255);
        BackColor = TransparencyKey;

        handle = this;

        Log("Initializing Overlay Paint Hooks..");

        // init sdl
        //{
        //    display = new PictureBox()
        //    {
        //        Dock = DockStyle.Fill
        //    };
        //    Controls.Add(display);

        //    if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) != 0)
        //        throw new Exception($"SDL_Init Error: {SDL.SDL_GetError()}");

        //    window = SDL.SDL_CreateWindow("SDL2 Triangle Example", 0, 0, 800, 600, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL);
        //    if (window == IntPtr.Zero)
        //        throw new Exception($"SDL_CreateWindow Error: {SDL.SDL_GetError()}");

        //    renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        //    if (renderer == IntPtr.Zero)
        //        throw new Exception($"SDL_CreateRenderer Error: {SDL.SDL_GetError()}");

        //    // move SDL window into winform as a mid child
        //    IsMdiContainer = true;

        //    // Make the SDL2 window a child of the WinForms form
        //    SetParent(window, this.Handle);
        //    // Set the window style to make it a child window
        //    SetWindowLong(window, -16, 0x40000000 | 0x02000000 | 0x04000000);
        //}

        Paint += OnUpdate;
        //Resize += OnResize;

        Log("Initializing Overlay Delegate..");

        Focus(); // bring the overlay window into focus (avoids it showing up behind the game)

        WinGraphics.SetClickthrough(this.Handle); // allows the user to click through the overlay even if its currently being drawn to

        WinGraphics.AddAdjust(this.Handle, OnAdjust); // Setup the adjust delegate for hooking

        Log("Initializing Overlay WinHooks..");

        WinGraphics.InitDelegate(this.Handle); // hook the window event via delegate

        //{
        //    // now we allow our other forms
        //    new ClickGui();
        //    //ClickGui.handle.Show();
        //}
    }

    private void OnResize(object sender, EventArgs e)
    {
        //SDL.SDL_SetWindowSize(window, Size.Width, Size.Height);
        //SDL.SDL_RenderSetLogicalSize(renderer, Size.Width, Size.Height);
    }

    public void OnUpdate(object sender, PaintEventArgs e)
    {
        //SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255); // clear form with nothing (Full transparency)
        //SDL.SDL_RenderClear(renderer);

        //// triangle
        //SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
        //SDL.SDL_RenderDrawLine(renderer, 400, 100, 200, 500);
        //SDL.SDL_RenderDrawLine(renderer, 200, 500, 600, 500);
        //SDL.SDL_RenderDrawLine(renderer, 600, 500, 400, 100);

        //SDL.SDL_RenderPresent(renderer);

        Graphics g = e.Graphics;

        // lets check if the cursor is visible if so then we dont draw a crosshair
        if (Keymap.CanUseMoveKeys() == true) // || Keymap.GetDown(Keys.Tab) later ig
            return;

        // lets quickly draw a test crosshair
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