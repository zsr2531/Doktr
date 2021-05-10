using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Doktr
{
    public static class Startup
    {
        #if RELEASE
        private const LogEventLevel DefaultLevel = LogEventLevel.Information;
        #else
        private const LogEventLevel DefaultLevel = LogEventLevel.Debug;
        #endif
        
        public static IServiceProvider ConfigureServices(DoktrConfiguration configuration, LogEventLevel? level = null)
        {
            var collection = new ServiceCollection();

            collection.AddSingleton(configuration);
            collection.AddSingleton(CreateLogger(level ?? DefaultLevel));
            
            return collection.BuildServiceProvider();
        }

        private static ILogger CreateLogger(LogEventLevel level)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}