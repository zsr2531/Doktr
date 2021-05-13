using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Doktr
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices(DoktrConfiguration configuration, ILogger logger)
        {
            var collection = new ServiceCollection();

            collection.AddSingleton(configuration);
            collection.AddSingleton(logger);
            
            return collection.BuildServiceProvider();
        }
    }
}