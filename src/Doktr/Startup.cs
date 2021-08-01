using System;
using Doktr.Services;
using Doktr.Services.DocumentationParsing;
using Doktr.Services.GraphTransformers;
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

            collection.AddSingleton<IAssemblyRepositoryService, AssemblyRepositoryService>();
            collection.AddSingleton<IMetadataResolutionService, MetadataResolutionService>();
            collection.AddSingleton<IGenericInstantiationService, GenericInstantiationService>();
            collection.AddSingleton<IDependencyGraphTransformerProvider, DependencyGraphTransformerProvider>();
            collection.AddSingleton<IDocumentationSegmentParserProvider, DocumentationSegmentParserProvider>();
            collection.AddSingleton<IXmlParserServiceFactory, XmlParserServiceFactory>();
            collection.AddSingleton<IDocumentIdTranslatorService, DocumentIdTranslatorService>();
            collection.AddTransient<IGraphBuilderService, GraphBuilderService>();
            
            return collection.BuildServiceProvider();
        }
    }
}