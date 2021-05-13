using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Doktr.CommandLine;
using Serilog;
using Serilog.Events;

namespace Doktr
{
    public static class Program
    {
        #if RELEASE
        private const LogEventLevel DefaultLevel = LogEventLevel.Information;
        #else
        private const LogEventLevel DefaultLevel = LogEventLevel.Verbose;
        #endif
        
        public static void Main(string[] args)
        {
            var cli = ParseCommandLine(args);
            PreTaskActions.RunActionsIfNeeded(cli);
            
            Run(cli);
        }

        private static void Run(CommandLineParseResult cli)
        {
            var logEventLevel = GetLogEventLevel(cli);
            var logger = CreateLogger(logEventLevel);
            var pipeline = CreatePipeline(cli, logger);
            var stopwatch = Stopwatch.StartNew();
            
            logger.Debug("Welcome to Doktr!");
            logger.Information("Doktr v0.0.1");

            if (pipeline.Length == 0)
                logger.Warning("Nothing to do.");

            foreach (var configuration in pipeline)
            {
                var provider = Startup.ConfigureServices(configuration, logger);
                RunConfiguration(configuration, provider);
            }
            
            stopwatch.Stop();

            var elapsed = stopwatch.Elapsed;
            logger.Information("Finished all jobs in {Elapsed}.", elapsed);
        }

        private static void RunConfiguration(DoktrConfiguration configuration, IServiceProvider serviceProvider)
        {
            // Step 1: Load assemblies using AsmResolver and add them to the workspace.
            // Step 2: Load the .xml files and parse them.
            // Step 3: Resolve all external references using xref services.
            // Step 4: Create a dependency graph between members to resolve inherited documentation.
            // Step 5: Resolve <inheritdoc />'s.
            // Step 6: Create output directories.
            // Step 7: Generate markdown.
        }

        private static CommandLineParseResult ParseCommandLine(string[] args)
        {
            var provider = CommandLineSwitchProvider.Instance;
            var parser = new CommandLineParser(provider);
            
            return parser.ParseCommandLine(args);
        }

        private static LogEventLevel GetLogEventLevel(CommandLineParseResult cli)
        {
            return cli.HasFlag(CommandLineSwitchProvider.Verbose)
                ? LogEventLevel.Debug
                : DefaultLevel;
        }

        private static ILogger CreateLogger(LogEventLevel eventLevel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(eventLevel)
                .WriteTo.Console()
                .CreateLogger();
        }

        private static ImmutableArray<DoktrConfiguration> CreatePipeline(CommandLineParseResult cli, ILogger logger)
        {
            var builder = ImmutableArray.CreateBuilder<DoktrConfiguration>();

            foreach (string inputFile in cli.Input)
            {
                if (!File.Exists(inputFile))
                {
                    logger.Error("Configuration file '{Path}' not found.", inputFile);
                    continue;
                }

                logger.Verbose("'{Path}' exists, trying to load it.", inputFile);
                var xml = new XmlSerializer(typeof(DoktrConfiguration));

                try
                {
                    using var stream = File.OpenRead(inputFile);
                    object? result = xml.Deserialize(stream);
                    if (result is not DoktrConfiguration configuration)
                        throw new InvalidDataException();

                    builder.Add(configuration);
                    logger.Debug("Successfully loaded '{Path}'.", inputFile);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occured while loading '{Path}'.", inputFile);
                }
            }

            logger.Verbose("Processed all valid input files.");
            return builder.ToImmutable();
        }
    }
}