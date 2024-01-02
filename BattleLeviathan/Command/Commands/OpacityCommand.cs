using System.Collections.Generic;

class OpacityCommand : Command
{
    public OpacityCommand() : base("opacity", "Set the DirectX hook opacity (meaning it'll fade a bit)", new List<string>() { "transparency" })
    { }

    public override bool execute(List<string> cmd)
    {
        if (cmd.Count > 0)
        {
            int outp = 0;

            if (int.TryParse(cmd[0], out outp))
                Overlay.handle.Opacity = outp;
            else Debug.Log("Invalid argument");
        }

        return false;
    }
}