using Doktr.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Doktr
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var cli = ParseCommandLine(args);
            PreTaskActions.RunActionsIfNeeded(cli);
            
            Run(cli);
        }

        private static void Run(CommandLineParseResult cli)
        {
            var configuration = CreateConfiguration(cli);
            var logEventLevel = GetLogEventLevel(cli);
            var provider = Startup.ConfigureServices(configuration, logEventLevel);
            var logger = provider.GetRequiredService<ILogger>();
            
            logger.Debug("Welcome to Doktr!");
            logger.Information("Doktr v0.0.1");
        }

        private static CommandLineParseResult ParseCommandLine(string[] args)
        {
            var provider = CommandLineSwitchProvider.Instance;
            var parser = new CommandLineParser(provider);
            
            return parser.ParseCommandLine(args);
        }

        private static LogEventLevel? GetLogEventLevel(CommandLineParseResult cli)
        {
            return cli.HasFlag(CommandLineSwitchProvider.Instance.Verbose)
                ? LogEventLevel.Verbose
                : null;
        }

        private static DoktrConfiguration CreateConfiguration(CommandLineParseResult cli)
        {
            return null!;
        }
    }
}