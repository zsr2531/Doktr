using Doktr.Xml.Collections;
using Doktr.Xml.XmlDoc.Collections;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Doktr.Xml.XmlDoc.SectionParsers;

namespace Doktr.Xml.XmlDoc;

// TODO: Introduce the concept of diagnostics instead of throwing for everything.
public partial class XmlDocParser : IXmlDocParser
{
    private const string Doc = "doc";
    private const string Assembly = "assembly";
    private const string Members = "members";
    private const string Names = "name";

    private readonly Dictionary<string, ISectionParser> _sectionParsers;
    private readonly Dictionary<string, IFragmentParser> _fragmentParsers;
    private readonly XmlNodeCollection _nodes;
    private int _position;

    public XmlDocParser(
        IEnumerable<ISectionParser> sectionParsers,
        IEnumerable<IFragmentParser> fragmentParsers,
        XmlNodeCollection nodes)
    {
        _sectionParsers = sectionParsers.ToDictionary(k => k.Tag, v => v);
        _fragmentParsers = fragmentParsers
                           .SelectMany(p => p.SupportedTags.Select(t => (p, t)))
                           .ToDictionary(k => k.t, v => v.p);
        _nodes = nodes;
    }

    private XmlNode LastNode => _nodes[^1];

    public RawXmlDocEntryMap ParseXmlDoc()
    {
        var map = new RawXmlDocEntryMap();
        bool hasPrologue = ParsePrologue();

        while (Lookahead is not XmlEndElementNode and not null)
        {
            var entry = ParseMember();
            map[entry.DocId] = entry;
        }

        if (hasPrologue)
            ParseEpilogue();

        return map;
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
}