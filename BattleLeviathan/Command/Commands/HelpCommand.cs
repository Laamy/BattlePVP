using SharpDX;
using System.Collections.Generic;

class HelpCommand : Command
{
    public HelpCommand() : base("commands", "Get a list of commands", new List<string>(){ "cmds", "help" })
    { }

    public override bool execute(List<string> cmd)
    {
        Debug.Log($"Commands ({BattlefieldClient.CommandManager._commands.Count})");

        foreach (Command command in BattlefieldClient.CommandManager._commands)
            Debug.Log($"{command.name} - {command.descr}");

        return false;
    }
}