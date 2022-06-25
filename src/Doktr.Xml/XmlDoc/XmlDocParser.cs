using Doktr.Core.Models;
using Doktr.Xml.Collections;
using Doktr.Xml.XmlDoc.Collections;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Doktr.Xml.XmlDoc.SectionParsers;
using Serilog;

namespace Doktr.Xml.XmlDoc;

// TODO: Don't log events inside the parser... code smell!
public partial class XmlDocParser : IXmlDocParser
{
    private const string Doc = "doc";
    private const string Assembly = "assembly";
    private const string Members = "members";
    private const string Names = "name";

    private readonly Dictionary<string, ISectionParser> _sectionParsers;
    private readonly Dictionary<string, IFragmentParser> _fragmentParsers;
    private readonly ILogger _logger;
    private readonly XmlNodeCollection _nodes;
    private int _position;
    private CodeReference? _current;

    public XmlDocParser(
        IEnumerable<ISectionParser> sectionParsers,
        IEnumerable<IFragmentParser> fragmentParsers,
        ILogger logger,
        XmlNodeCollection nodes)
    {
        _sectionParsers = sectionParsers.ToDictionary(k => k.Tag, v => v);
        _fragmentParsers = fragmentParsers
                           .SelectMany(p => p.SupportedTags.Select(t => (p, t)))
                           .ToDictionary(k => k.t, v => v.p);
        _logger = logger;
        _nodes = nodes;
    }

    public bool HasIssues => !Diagnostics.IsEmpty;
    public bool HasErrors => Diagnostics.HasErrors;
    public XmlDocDiagnosticCollection Diagnostics { get; } = new();

    public RawXmlDocEntryMap ParseXmlDoc()
    {
        var map = new RawXmlDocEntryMap();
        bool hasPrologue = ParsePrologue();

        while (Lookahead.IsNotEndElementOrEof())
        {
            try
            {
                ParseMember(map);
            }
            catch (XmlDocParserException ex)
            {
                ReportDiagnostic(XmlDocDiagnostic.MakeError(ex.Span, ex.Message));
                RecoverToNextMember();
            }
            finally
            {
                _current = null;
            }
        }

        if (hasPrologue)
            ParseEpilogue();

        return map;
    }

    public void ReportDiagnostic(XmlDocDiagnostic diagnostic)
    {
        Diagnostics.Add(diagnostic);
        if (_current is not null)
            ReportDiagnosticMember();
        else
            ReportDiagnosticNormal();

        void ReportDiagnosticNormal()
        {
            if (diagnostic.Severity == XmlDocDiagnosticSeverity.Error)
                _logger.Error("An error occurred while parsing XML documentation: {Diagnostic}", diagnostic);
            else
                _logger.Warning("There was an issue while parsing XML documentation: {Diagnostic}", diagnostic);
        }

        void ReportDiagnosticMember()
        {
            if (diagnostic.Severity == XmlDocDiagnosticSeverity.Error)
            {
                _logger.Error("An error occurred while parsing XML documentation for '{Member}': {Diagnostic}",
                    _current,
                    diagnostic);
            }
            else
            {
                _logger.Warning("There was an issue while parsing XML documentation for '{Member}': {Diagnostic}",
                    _current,
                    diagnostic);
            }
        }
    }

    private bool ParsePrologue()
    {
        if (Lookahead is not XmlElementNode { Name: Doc })
            return false;

        ExpectElement(Doc);
        ExpectElement(Assembly);
        ExpectElement(Names);
        ExpectText();
        ExpectEndElement(Names);
        ExpectEndElement(Assembly);
        ExpectElement(Members);
        return true;
    }

    private void ParseEpilogue()
    {
        ExpectEndElement(Members);
        ExpectEndElement(Doc);
    }

    private void RecoverToNextMember()
    {
        while (!IsRecoveryPoint(Lookahead))
            Consume();

        static bool IsRecoveryPoint(XmlNode node) => node is XmlElementNode { Name: Member }
            or XmlEndElementNode { Name: Members } or XmlEndOfFileNode;
    }
}