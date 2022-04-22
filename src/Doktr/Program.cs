using System.Text.Json;
using Autofac;
using CommandLine;
using Doktr;
using Doktr.Core;
using Doktr.Decompiler;
using MediatR;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Parser.Default.ParseArguments<CommandLineOptions>(args)
      .WithParsed(Start);

static void Start(CommandLineOptions options)
{
    try
    {
        var container = CreateContainer(options);
        var logger = container.Resolve<ILogger>();
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine("An unexpected error occurred:\n" + ex);
    }
}

static IContainer CreateContainer(CommandLineOptions options)
{
    var logger = CreateLogger(options.Verbose);
    var configuration = LoadDoktrConfiguration(options.ProjectFilePath, logger);
    var container = new ContainerBuilder();

    container.RegisterInstance(logger);
    container.RegisterInstance(configuration);

    container.RegisterGeneric(typeof(LoggerPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>)).SingleInstance();

    container.RegisterAssemblyTypes(typeof(DecompileMemberHandler).Assembly)
             .AsImplementedInterfaces();

    return container.Build();
}

static ILogger CreateLogger(bool verbose)
{
    var configuration = new LoggerConfiguration();
    configuration.MinimumLevel.Is(verbose ? LogEventLevel.Verbose : LogEventLevel.Debug);
    configuration.WriteTo.Console(theme: AnsiConsoleTheme.Code);

    return configuration.CreateLogger();
}

static DoktrConfiguration LoadDoktrConfiguration(string path, ILogger logger)
{
    try
    {
        using var file = File.OpenRead(path);
        return JsonSerializer.Deserialize<DoktrConfiguration>(file)!;
    }
    catch (Exception ex)
    {
        logger.Fatal(ex, "Failed to load configuration file");
        throw;
    }
}