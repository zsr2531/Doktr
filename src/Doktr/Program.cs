using System;
using System.Text;
using AsmResolver.DotNet;
using Doktr.Analysis;
using Doktr.CommandLine;

namespace Doktr
{
    public static class Program
    {
        private const string Version = "1.0.0.0";
        private const string RepoUrl = "https://github.com/zsr2531/Doktr";
        private const string Usage = "dotnet Doktr.dll [options] <asm:xml>...";
            
        public static void Main(string[] args)
        {
            var arguments = new CommandLineParser(args).Parse();
            if (arguments.HasFlag(CommandLineSwitches.Help))
            {
                PrintHelpMessage();
                return;
            }

            foreach (var target in arguments.TargetFiles)
            {
                var module = ModuleDefinition.FromFile(target.Assembly);
                var result = new DependencyGraphBuilder(module).BuildDependencyGraph();
            }
        }

        private static void PrintHelpMessage()
        {
            var sb = new StringBuilder($"Doktr v{Version}\n{RepoUrl}\n\nUSAGE: {Usage}\n\nCommand line options:\n");

            foreach (var @switch in CommandLineSwitches.Switches)
            {
                string identifiers = "   " + string.Join(", ", @switch.Identifiers);
                sb.Append(identifiers.PadRight(25));
                sb.Append(@switch.Description);

                if (@switch.HasValue)
                    sb.AppendLine($" (default: {@switch.DefaultValue})");
                else
                    sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }
    }
}