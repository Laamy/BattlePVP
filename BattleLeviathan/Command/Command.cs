using System.Collections.Generic;

class Command
{
    public Command(string name, string descr, List<string> aliases)
    {
        this.name = name;
        this.descr = descr;
        this.aliases = aliases;
    }

    public virtual bool execute(List<string> cmd) { return false; }

    public string name = "";
    public string descr = "";
    public List<string> aliases;
};