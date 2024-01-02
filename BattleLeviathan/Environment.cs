using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

class Environment
{
    private static string data = Application.StartupPath + "\\BattleLeviathan_Data\\";

    private static string bin = data + "bin\\";

    private static string assets = data + "assets\\";
    private static string fonts = assets + "fonts\\";

    public static List<System.Drawing.FontFamily> _fonts = new List<System.Drawing.FontFamily>();

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
        //LoadFonts();
    }

    private static void LoadFont(string file)
    {
        var fontCollection = new PrivateFontCollection();
        fontCollection.AddFontFile(fonts + file);

        _fonts.Add(fontCollection.Families[0]);

        Debug.Log(_fonts.LastOrDefault().Name);
    }

    private static void LoadFonts()
    {
        LoadFont("vgasys.fon");

        //var customFontTextFormat = new TextFormat(new Factory(), fontCollection.Families[0].Name, FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 12);
    }
}