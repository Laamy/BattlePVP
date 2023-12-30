using System.Windows.Forms;

class Program
{
    static void Main(string[] args)
    {
        // get the information we need from rust for our WinHooks
        BattlefieldClient.OpenGame();

        // just gonna start it cuz we're not injecting anything so no need for multi-thread
        Application.Run(new Overlay());
    }
}