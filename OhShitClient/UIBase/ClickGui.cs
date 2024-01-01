using System;
using System.Drawing;
using System.Windows.Forms;

using static BattlefieldClient;
using static User32;
using static Debug;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace OhShitClient.UIBase
{
    public partial class ClickGui : Form
    {
        public static ClickGui handle;

        public Point relative;

        public ClickGui()
        {
            Log("Initializing ClickGui UI..");

            InitializeComponent();
            InitializeCustom();
            TopMost = true;
            //InitializeCustom();

            handle = this;

            Log("Initializing ClickGui Event Hooks..");

            Move += OnMove;
            //Paint += OnUpdate;

            Log("Initializing ClickGui Delegate..");

            Focus(); // bring the clcikgui window into focus (avoids it showing up behind the game)

            WinGraphics.SetClickthrough(this.Handle);
            //FormBorderStyle = FormBorderStyle.None;

            WinGraphics.AddAdjust(this.Handle, OnAdjust); // Setup the adjust delegate for hooking

            Log("Initializing ClickGui WinHooks..");

            WinGraphics.InitDelegate(this.Handle); // hook the window event via delegate

            {
                ProcessRectangle rect = GetGameRect();

                int x = rect.Left;
                int y = rect.Top;

                Move -= OnMove;
                relative = new Point(10, 40);
                Location = new Point(x + relative.X, y + relative.Y);
                Move += OnMove;
            }

            MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, 0xA1, 0x2, 0);
                }
            };
        }

        public void InitializeCustom()
        {
            // initialize winform here
            TopMost = true; // not needed
            TransparencyKey = BackColor;

            FormBorderStyle = FormBorderStyle.None;

            Text = GameTitle;

            DoubleBuffered = true; // no tearing cuz this is gay
        }

        private void OnMove(object sender, EventArgs e)
        {
            if (relative == null)
                return;

            ProcessRectangle rect = GetGameRect();

            int x = rect.Left;
            int y = rect.Top;

            Point newLoc = new Point(Location.X - x, Location.Y - y);

            relative.X += newLoc.X - relative.X;
            relative.Y += newLoc.Y - relative.Y;

            Console.WriteLine(relative);
        }

        //ProcessRectangle old;

        public void OnAdjust(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // get the rough dimensions of the window
            ProcessRectangle rect = GetGameRect();
            //rect = old;

            // Doesnt support maximized window
            int x = rect.Left; // this should cover when your dragging the window around and alt + enter (f11) fullscreen
            int y = rect.Top;

            if (Location != new Point(x + relative.X, y + relative.Y))
            {
                Move -= OnMove;
                SetWindowPos(Handle, IsGameFocusedInsert(), x + relative.X, y + relative.Y, Size.Width, Size.Height, 0x40);
                Move += OnMove;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("CLICK");
        }
    }
}
