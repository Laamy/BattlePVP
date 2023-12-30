using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class Program
{
    public static EventHandler BackgroundTick;

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
                    BackgroundTick.Invoke(null, null);

                // check if the game closed
                if (Process.GetProcessesByName(BattlefieldClient.GameName).Length < 1)
                    Process.GetCurrentProcess().Kill();
            }
        });

        // just gonna start it cuz we're not injecting anything so no need for multi-thread
        Application.Run(new Overlay());
    }
}