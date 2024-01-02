using System.IO;
using System.Reflection;
using System.Runtime.Hosting;
using System.Windows.Forms;

class Environment
{
    private static string data = "BattleLeviathan_Data\\";

    private static string bin = data + "bin\\";

    private static void LoadBin()
    {
        // move paths real quick to the binaries folder
        Kernel32.SetDllDirectory(bin);

        // load in from binary folder
        //if (Directory.Exists(bin))
        //{
        //    foreach (var dllFile in Directory.GetFiles(bin, "*.dll"))
        //    {
        //        Assembly.LoadFile($"{Application.StartupPath}\\{dllFile}");
        //    }
        //}
    }

    public static void Initialize()
    {
        LoadBin();
    }
}