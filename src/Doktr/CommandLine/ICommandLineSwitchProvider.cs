using System.Collections.Immutable;

namespace Doktr.CommandLine
{
    public interface ICommandLineSwitchProvider
    {
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