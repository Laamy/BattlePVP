using System.Collections.Generic;

class ClearColourCommand : Command
{
    public ClearColourCommand() : base("clearcolour", "Set the DirectX hook clear colour", new List<string>() { "cc", "clearcolor", "ccolour", "ccolor" })
    { }

    public override bool execute(List<string> cmd)
    {
        if (cmd.Count > 2)
        {
            int R = 0;
            int G = 0;
            int B = 0;

            if (
                int.TryParse(cmd[0], out R) &&
                int.TryParse(cmd[1], out G) &&
                int.TryParse(cmd[2], out B))
                Overlay.handle.context.clearColour = new SharpDX.Color4(R / 255, G / 255, B / 255, 1);
            else Debug.Log("Invalid argument");
        }

        return false;
    }
}