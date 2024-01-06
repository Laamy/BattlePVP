using System.Collections.Generic;
using System.Text.RegularExpressions;

class CommandManager
{
    public List<Command> _commands = new List<Command>();

    public void Init()
    {
        _commands.Add(new HelpCommand());
        _commands.Add(new OpacityCommand());
        _commands.Add(new ClearColourCommand());
        _commands.Add(new ShaderPackCommand());
    }

    public bool SendCommand(string cmd)
    {
        Command cur = null;

        if (cmd.Length < 2)
            return true;

        string commandName = "";
        List<string> arguments = ParseArguments(cmd, out commandName);

        foreach (Command command in _commands)
        {
            if (command.name == commandName.ToLower() || command.aliases.Contains(commandName.ToLower()))
            {
                cur = command;
                break;
            }
        }

        if (cur == null)
        {
            Debug.Log("Invalid command");
            return true;
        }

        cur.execute(arguments);

        return true;
    }

    static List<string> ParseArguments(string input, out string commandName)
    {
        List<string> arguments = new List<string>();
        Regex regex = new Regex(@"[^\s""]+|""([^""]*)""");

        MatchCollection matches = regex.Matches(input);

        // The first argument is considered the command name
        commandName = matches.Count > 0 ? matches[0].Value : "";

        if (matches.Count > 1)
        {
            // If there are additional arguments, only consider the first one
            arguments.Add(matches[1].Groups[1].Success ? matches[1].Groups[1].Value : matches[1].Value);
        }

        return arguments;
    }
}