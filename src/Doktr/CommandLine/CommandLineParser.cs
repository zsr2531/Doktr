using System;
using System.Collections.Immutable;

namespace Doktr.CommandLine;

public class CommandLineParser
{
    private readonly ICommandLineSwitchProvider _switchProvider;

    public CommandLineParser(ICommandLineSwitchProvider switchProvider)
    {
        _switchProvider = switchProvider;
    }

    public CommandLineParseResult ParseCommandLine(string[] args)
    {
        var flags = ImmutableHashSet.CreateBuilder<CommandLineSwitch>();
        var options = ImmutableDictionary.CreateBuilder<CommandLineSwitch, string>();
        var input = ImmutableArray.CreateBuilder<string>();

        for (int i = 0; i < args.Length; i++)
        {
            string current = args[i];

            if (IsFlag(current))
            {
                flags.Add(_switchProvider.Flags[current]);
            }
            else if (IsOption(current))
            {
                var option = _switchProvider.Options[current];
                string? value = GetValue(args, ref i);

                if (value is null)
                    Console.Error.WriteLine($"No value provided for option '{current}'... ignoring.");
                else if (options.ContainsKey(option))
                    Console.Error.WriteLine(
                        $"A value was already provided for option '{current}'... ignoring new value.");
                else
                    options.Add(option, value);
            }
            else
            {
                input.Add(current);
            }
        }

        return new CommandLineParseResult(flags.ToImmutable(), options.ToImmutable(), input.ToImmutable());
    }

    private string? GetValue(string[] args, ref int i)
    {
        return i + 1 >= args.Length
            ? null
            : args[++i];
    }

    private bool IsFlag(string raw) => _switchProvider.Flags.ContainsKey(raw);

    private bool IsOption(string raw) => _switchProvider.Options.ContainsKey(raw);
}