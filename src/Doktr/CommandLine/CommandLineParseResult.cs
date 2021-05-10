using System;
using System.Collections.Immutable;

namespace Doktr.CommandLine
{
    public readonly ref struct CommandLineParseResult
    {
        private readonly ImmutableHashSet<CommandLineSwitch> _flags;
        private readonly ImmutableDictionary<CommandLineSwitch, string> _options;

        public CommandLineParseResult(
            ImmutableHashSet<CommandLineSwitch> flags,
            ImmutableDictionary<CommandLineSwitch, string> options,
            ImmutableArray<string> input)
        {
            Input = input;
            _flags = flags;
            _options = options;
        }

        public ImmutableArray<string> Input
        {
            get;
        }

        public bool HasFlag(CommandLineSwitch sw) => _flags.Contains(sw);

        public string GetOption(CommandLineSwitch sw)
        {
            if (sw.DefaultValue is null)
                throw new InvalidOperationException("Cannot get the value of a flag.");

            return _options.TryGetValue(sw, out var value)
                ? value
                : sw.DefaultValue;
        }
    }
}