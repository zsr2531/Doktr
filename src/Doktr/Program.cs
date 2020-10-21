using System;
using System.Text;
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
        }

        private static void PrintHelpMessage()
        {
            var sb = new StringBuilder($"Doktr v{Version}\n{RepoUrl}\n\nUSAGE: {Usage}\n\nCommand line options:\n");

            foreach (var @switch in CommandLineSwitches.Switches)
            {
            }

            Console.WriteLine(sb.ToString());
        }
    }
}