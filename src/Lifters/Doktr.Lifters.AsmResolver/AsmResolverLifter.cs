using AsmResolver.DotNet;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Lifters.Common;
using Doktr.Lifters.Common.DependencyGraph;
using Doktr.Xml;
using Doktr.Xml.Collections;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.Collections;
using Serilog;

namespace Doktr.Lifters.AsmResolver;

public class AsmResolverLifter : IModelLifter
{
    private readonly ILogger _logger;
    private readonly Func<TextReader, IXmlParser> _xmlParserFactory;
    private readonly Func<XmlNodeCollection, IXmlDocParser> _xmlDocParserFactory;
    private readonly Func<IEnumerable<ModuleDefinition>, IDependencyGraphBuilder<IMemberDefinition>>
        _depGraphBuilderFactory;

    public AsmResolverLifter(
        ILogger logger,
        Func<TextReader, IXmlParser> xmlParserFactory,
        Func<XmlNodeCollection, IXmlDocParser> xmlDocParserFactory,
        Func<IEnumerable<ModuleDefinition>, IDependencyGraphBuilder<IMemberDefinition>> depGraphBuilderFactory)
    {
        _logger = logger;
        _xmlParserFactory = xmlParserFactory;
        _xmlDocParserFactory = xmlDocParserFactory;
        _depGraphBuilderFactory = depGraphBuilderFactory;
    }

    public AssemblyTypesMap LiftModels(IEnumerable<DoktrTarget> targets)
    {
        var modules = new List<ModuleDefinition>();
        var docs = new RawXmlDocEntryMap();

        foreach (var target in targets)
        {
            var module = ModuleDefinition.FromFile(target.AssemblyPath);
            modules.Add(module);
            docs.MergeWith(ParseXmlDoc(target));
        }

        // return new ModelLifterResult(module.Assembly!.FullName, _types);
        return new AssemblyTypesMap();
    }

    private RawXmlDocEntryMap ParseXmlDoc(DoktrTarget target)
    {
        var reader = new StreamReader(target.XmlPath);
        var xmlParser = _xmlParserFactory(reader);
        var nodes = xmlParser.ParseXmlNodes();

        WriteXmlDiagnostics(xmlParser.Diagnostics);
        var xmlDocParser = _xmlDocParserFactory(nodes);
        return xmlDocParser.ParseXmlDoc();
    }

    private void WriteXmlDiagnostics(XmlDiagnosticCollection diagnostics)
    {
        if (diagnostics.IsEmpty)
            return;

        foreach (var diagnostic in diagnostics)
            _logger.Error("An error occurred while parsing xml: {Diagnostic}", diagnostic);
    }

    private void ResolveInheritance(List<ModuleDefinition> modules, RawXmlDocEntryMap docs)
    {
        var depGraph = BuildDependencyGraph(modules);
        var cache = new RawXmlDocEntryMap();

        foreach (var entry in docs)
        {
        }
    }

    private RawXmlDocEntry ResolveInheritance(
        DependencyGraph<IMemberDefinition> depGraph,
        RawXmlDocEntry entry,
        RawXmlDocEntryMap docs,
        RawXmlDocEntryMap cache)
    {
        return new(new CodeReference("T:Test"));
    }

    private DependencyGraph<IMemberDefinition> BuildDependencyGraph(List<ModuleDefinition> modules)
    {
        var builder = _depGraphBuilderFactory(modules);
        var depGraph = builder.BuildDependencyGraph();
        builder.PerformAnalysis();

        return depGraph;
    }
}