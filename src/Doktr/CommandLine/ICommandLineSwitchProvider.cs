using System.Collections.Immutable;

namespace Doktr.CommandLine
{
    public interface ICommandLineSwitchProvider
    {
        ImmutableArray<CommandLineSwitch> AllSwitches
        {
            get;
        }

        ImmutableDictionary<string, CommandLineSwitch> Flags
        {
            get;
        }

        ImmutableDictionary<string, CommandLineSwitch> Options
        {
            get;
        }
    }
}