using System.Collections.Generic;

class ShaderPackCommand : Command
{
    public ShaderPackCommand() : base("shaderpack", "set some predefined shader packs", new List<string>() { "sp", "shader", "shaders", "shaderspack" })
    { }

    public override bool execute(List<string> cmd)
    {
        if (cmd.Count == 1)
        {
            if (cmd[0] == "" || cmd[0] == "default" || cmd[0] == "vanilla")
                Overlay.AllowShaders = false;
            else if (cmd[0] == "bluenoise")
                Overlay.AllowShaders = true;
        }

        return false;
    }
}