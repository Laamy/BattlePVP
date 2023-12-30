using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class Program
{
    public static EventHandler<EventArgs> BackgroundTick;

    static void Main(string[] args)
    {
        // get the information we need from rust for our WinHooks
        BattlefieldClient.OpenGame();

        Task.Factory.StartNew(() =>
        {
            while (true) // Background thread stuff
            {
                // C# likes to use all the cpu so lets limit it to once per 15ms (as that what the C# timer is normally set to)
                Thread.Sleep(1);

                // tick all the background stuff like keymap
                if (BackgroundTick != null)
                    BackgroundTick.Invoke(null, new EventArgs());

                // check if the game closed
                if (Process.GetProcessesByName(BattlefieldClient.GameName).Length < 1)
                    Process.GetCurrentProcess().Kill();
            }
        });

        new Keymap(); // init keymap
        Keymap.keyEvent += OnKey;

        // just gonna start it cuz we're not injecting anything so no need for multi-thread
        Application.Run(new Overlay());
    }

    private static void OnKey(object sender, KeyEvent e)
    {
        if (e.vkey != VKeyCodes.KeyHeld)
            Console.WriteLine($"{e.vkey} {e.key}");
    }
}