using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

class Program
{
    public static bool CmdBar = false;

    static void Main()
    {
        // init binarys n assets
        Environment.Initialize();

        // setup battlefield interface class
        BattlefieldClient.CreateInstance();

        Task.Factory.StartNew(() => // runs 30 times a second
        {
            while (true) // Background thread stuff
            {
                // C# likes to use all the cpu so lets limit it to once per 30th of a second
                Thread.Sleep(1000 / 30);

                // check if the game closed
                if (Process.GetProcessesByName(BattlefieldClient.GameName).Length < 1)
                    Process.GetCurrentProcess().Kill();

                // redraw the overlay regardless now as its DirectX
                if (Overlay.handle != null)
                    Overlay.handle.Invalidate();

                // tick all the background stuff like keymap
                BattlefieldClient.Keymap.Tick();
            }
        });

        BattlefieldClient.Keymap.OnKeyEvent += OnKey;

        // init overlay
        Application.Run(new Overlay());
    }

    private static void OnKey(object sender, KeyEvent e)
    {
        if (e.vkey != VKeyCodes.KeyHeld)
            Debug.Log($"{e.vkey} {e.key}:{(int)e.key}");

        if (e.vkey == VKeyCodes.KeyDown)
        {
            if (e.key == Keys.L && BattlefieldClient.Keymap.GetDown(Keys.ControlKey))
                Process.GetCurrentProcess().Kill(); 

            if (e.key == Keys.F1)
                CmdBar = !CmdBar;

            if (CmdBar) // textbox stuff
            {
                if (e.key.ToString().Length == 1)
                {
                    if (BattlefieldClient.Keymap.GetDown(Keys.ShiftKey))
                        Overlay.command += e.key.ToString().ToUpper();
                    else
                        Overlay.command += e.key.ToString().ToLower();
                }

                if (e.key == Keys.Space)
                    Overlay.command += " ";

                if (e.key == Keys.Back && Overlay.command.Length > 0)
                    Overlay.command = Overlay.command.Substring(0, Overlay.command.Length - 1);

                if (e.key == Keys.Enter)
                {
                    BattlefieldClient.CommandManager.SendCommand(Overlay.command);
                    Overlay.command = "";
                }
            }
        }
    }
}