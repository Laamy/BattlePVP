using System.Windows.Forms;

class Program
{
    static void Main()
    {
        // init binarys n assets
        Environment.Initialize();

        // example code
        Application.Run(new DirectXOverlay());
    }
}