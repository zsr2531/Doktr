using AsmResolver.DotNet;
using Doktr.Core.Models.Collections;
using Doktr.Lifters.Common;
using Doktr.Xml;
using Doktr.Xml.Collections;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.Collections;
using Serilog;

namespace Doktr.Lifters.AsmResolver;

public class AsmResolverLifter : IModelLifter
{
    private readonly ILogger _logger;
    private readonly ModuleDefinition _module;
    private readonly RawXmlDocEntryMap _xmlDoc;
    private readonly TypeDocumentationCollection _types = new();

    public AsmResolverLifter(
        string assemblyPath,
        string xmlPath,
        ILogger logger,
        Func<TextReader, IXmlParser> xmlParserFactory,
        Func<XmlNodeCollection, IXmlDocParser> xmlDocParserFactory)
    {
        _logger = logger;
        _module = ModuleDefinition.FromFile(assemblyPath);
        _xmlDoc = ParseXmlDoc(xmlPath, xmlParserFactory, xmlDocParserFactory);
    }

    public TypeDocumentationCollection LiftModels()
    {
        return _types;
    }

    private RawXmlDocEntryMap ParseXmlDoc(
        string xmlPath,
        Func<TextReader, IXmlParser> xmlParserFactory,
        Func<XmlNodeCollection, IXmlDocParser> xmlDocParserFactory)
    {
        var reader = new StreamReader(xmlPath);
        var xmlParser = xmlParserFactory(reader);
        var nodes = xmlParser.ParseXmlNodes();

        WriteXmlDiagnostics(xmlParser.Diagnostics);
        var xmlDocParser = xmlDocParserFactory(nodes);
        return xmlDocParser.ParseXmlDoc();
    }

    private void WriteXmlDiagnostics(XmlDiagnosticCollection diagnostics)
    {
        if (diagnostics.IsEmpty)
            return;

        foreach (var diagnostic in diagnostics)
        {
            _logger.Error("An error occurred while parsing xml: {Diagnostic}", diagnostic);
        }
    }
}