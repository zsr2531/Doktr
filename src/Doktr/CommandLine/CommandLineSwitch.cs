using System.Collections.Generic;
using System.Collections.Immutable;

namespace Doktr.CommandLine
{
    public class CommandLineSwitch
    {
        public CommandLineSwitch(IEnumerable<string> identifiers, string description, string defaultValue = default)
        {
            Identifiers = identifiers.ToImmutableArray();
            Description = description;
            DefaultValue = defaultValue;
            CommandLineSwitches.Switches.Add(this);
        }

        public bool HasValue => DefaultValue is not null;

        public ImmutableArray<string> Identifiers
        {
            get;
        }

        public string Description
        {
            get;
        }

        public string DefaultValue
        {
            get;
        }
    }
}