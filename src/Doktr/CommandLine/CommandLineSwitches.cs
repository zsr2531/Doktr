using System.Collections.Generic;

namespace Doktr.CommandLine
{
    public static class CommandLineSwitches
    {
        public static readonly IList<CommandLineSwitch> Switches = new List<CommandLineSwitch>();
        
        public static readonly CommandLineSwitch Help = new(new []
        {
            "-h", "--help"
        }, "Shows this help message.");
    }
}