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
      .WithParsed(result => Start(result).GetAwaiter().GetResult());

static async Task Start(CommandLineOptions options)
{
    try
    {
        var container = CreateContainer(options);
        var logger = container.Resolve<ILogger>();
        var mediator = container.Resolve<IMediator>();
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine("An unexpected error occurred:\n" + ex);
        Environment.ExitCode = 1; // Needed in order for CI to fail the build
    }
}

static IContainer CreateContainer(CommandLineOptions options)
{
    var logger = CreateLogger(options.Verbose);
    var configuration = LoadDoktrConfiguration(options.ProjectFilePath);
    var builder = new ContainerBuilder();

    // Singleton stuff
    builder.RegisterInstance(logger);
    builder.RegisterInstance(configuration);

    // Stuff needed for MediatR
    builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
    builder.RegisterGeneric(typeof(LoggerPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>)).SingleInstance();
    builder.Register<ServiceFactory>(ctx =>
    {
        var c = ctx.Resolve<IComponentContext>();
        return t => c.Resolve(t);
    });

    // MediatR handlers
    builder.RegisterAssemblyTypes(typeof(DecompileMemberHandler).Assembly)
           .AsImplementedInterfaces();

    return builder.Build();
}

static ILogger CreateLogger(bool verbose)
{
    var configuration = new LoggerConfiguration();
    configuration.MinimumLevel.Is(verbose ? LogEventLevel.Verbose : LogEventLevel.Debug);
    configuration.WriteTo.Console(theme: AnsiConsoleTheme.Code);

    return configuration.CreateLogger();
}

static DoktrConfiguration LoadDoktrConfiguration(string path)
{
    using var file = File.OpenRead(path);
    return JsonSerializer.Deserialize<DoktrConfiguration>(file)!;
}