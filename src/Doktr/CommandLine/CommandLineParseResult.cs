using System.Collections.Generic;
using System.Diagnostics;

namespace Doktr.CommandLine
{
    public class CommandLineParseResult
    {
        public CommandLineParseResult(
            ICollection<CommandLineSwitch> flags,
            IReadOnlyDictionary<CommandLineSwitch, string> options,
            IReadOnlyList<TargetFiles> targetFiles)
        {
            Flags = flags;
            Options = options;
            TargetFiles = targetFiles;
        }

        public ICollection<CommandLineSwitch> Flags
        {
            get;
        }

        public IReadOnlyDictionary<CommandLineSwitch, string> Options
        {
            get;
        }

        public IReadOnlyList<TargetFiles> TargetFiles
        {
            get;
        }

        public bool HasFlag(CommandLineSwitch flag)
        {
            Debug.Assert(!flag.HasValue);
            return Flags.Contains(flag);
        }

        public string GetOption(CommandLineSwitch option)
        {
            Debug.Assert(option.HasValue);
            return Options.TryGetValue(option, out string value)
                ? value
                : option.DefaultValue;
        }
    }
}