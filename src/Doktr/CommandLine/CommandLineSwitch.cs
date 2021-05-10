using System.Collections.Immutable;

namespace Doktr.CommandLine
{
    public class CommandLineSwitch
    {
        public CommandLineSwitch(string description, string? defaultValue, params string[] identifiers)
        {
            Description = description;
            Identifiers = ImmutableArray.Create(identifiers);
            DefaultValue = defaultValue;
        }

        public ImmutableArray<string> Identifiers
        {
            get;
        }
        
        public string Description
        {
            get;
        }

        public string? DefaultValue
        {
            get;
        }
    }
}