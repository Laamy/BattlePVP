using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class Program
{
    public static EventHandler<EventArgs> BackgroundTick;

    // igmnore this

    /*
     
     Thread.ProcessorCount 4
     Thread.MaxProcessorCount 4
     Thread.MinFreeProcessorCount 0
     Thread.JobThreadPriority 0
     GstRender.Thread.MaxProcessorCount 2
     
     */

    static void Main(string[] args)
    {
        // change DLL binary paths rq
        Kernel32.SetDllDirectory("BattlePVP_Data\\bin");

        // get the information we need from rust for our WinHooks
        BattlefieldClient.CreateInstance();
        bool dsa = false;

        bool lastVis = BattlefieldClient.CanUseMoveKeys;
        Task.Factory.StartNew(() => // runs 66 & a half times a second
        {
            while (true) // Background thread stuff
            {
                // C# likes to use all the cpu so lets limit it to once per 15ms (as that what the C# timer is normally set to)
                Thread.Sleep(1);

                // check if the game closed
                if (Process.GetProcessesByName(BattlefieldClient.GameName).Length < 1)
                    Process.GetCurrentProcess().Kill();

                // debug stuff
                {
                    //Console.WriteLine(Keymap.IsCursorVisible());
                }

                if (Overlay.handle != null && lastVis != BattlefieldClient.CanUseMoveKeys)
                {
                    Overlay.handle.Invalidate();
                    lastVis = BattlefieldClient.CanUseMoveKeys;
                }

                // tick all the background stuff like keymap
                if (BackgroundTick != null)
                    BackgroundTick.Invoke(null, new EventArgs());
            }
        });

        BattlefieldClient.Keymap.OnKeyEvent += OnKey;

        // init overlay
        Application.Run(new Overlay());
    }

    private static void OnKey(object sender, KeyEvent e)
    {
        if (e.vkey != VKeyCodes.KeyHeld)
            Console.WriteLine($"{e.vkey} {e.key}:{(int)e.key}");

        if (e.vkey == VKeyCodes.KeyDown)
        {
            if (e.key == Keys.L && Keymap.GetDown(Keys.ControlKey))
                Process.GetCurrentProcess().Kill(); // "eject"

            if (e.key == Keys.NumPad0)
                BattlefieldClient.SendCommand("Render.ResolutionScale 0.1"); // test rq
        }
    }
}