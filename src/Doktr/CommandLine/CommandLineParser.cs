using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Doktr.CommandLine
{
    public class CommandLineParser
    {
        private readonly string[] _args;

        private static readonly Dictionary<string, CommandLineSwitch> Flags = new();
        private static readonly Dictionary<string, CommandLineSwitch> Options = new();
        
        public CommandLineParser(string[] args)
        {
            _args = args;
        }

        static CommandLineParser()
        {
            foreach (var @switch in CommandLineSwitches.Switches)
            {
                var target = @switch.HasValue
                    ? Options
                    : Flags;
                
                foreach (string identifier in @switch.Identifiers)
                    target.Add(identifier, @switch);
            }
        }

        public CommandLineParseResult Parse()
        {
            var flags = new HashSet<CommandLineSwitch>();
            var options = new Dictionary<CommandLineSwitch, string>();
            var targets = new List<TargetFiles>();

            for (int i = 0; i < _args.Length; i++)
            {
                string current = _args[i];
                if (Flags.TryGetValue(current, out var flag))
                {
                    flags.Add(flag);
                }
                else if (Options.TryGetValue(current, out var option))
                {
                    if (i + 1 >= _args.Length)
                        throw new CommandLineParseException($"No value was provided for option '{current}'");

                    string param = _args[++i];
                    options.Add(option, param);
                }
                else
                {
                    var data = current.Split(':');
                    if (data.Length != 2)
                        throw new CommandLineParseException($"Unknown flag/option/target: '{current}'");
                    
                    Debug.Assert(data[1].EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
                    Debug.Assert(data[0].EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ||
                        data[0].EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
                    
                    targets.Add(new TargetFiles(data[0], data[1]));
                }
            }

            return new CommandLineParseResult(flags, options, targets);
        }
    }
}