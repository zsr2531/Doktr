using Doktr.Core.Models;
using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;

namespace Doktr.Xml.XmlDoc;

public partial class XmlDocParser
{
    public DocumentationFragment NextFragment()
    {
        return Lookahead switch
        {
            XmlTextNode => new TextFragment(Consume<XmlTextNode>().Text),
            XmlElementNode { Name: { } s } => GetFragmentParser(s).ParseFragment(this),
            XmlEmptyElementNode { Name: { } s } => GetFragmentParser(s).ParseFragment(this),
            _ => throw new XmlDocParserException($"Unexpected node: {Lookahead}", Consume().Span)
        };

        IFragmentParser GetFragmentParser(string tag)
        {
            if (_fragmentParsers.TryGetValue(tag, out var fragmentParser))
                return fragmentParser;

            throw new XmlDocParserException($"Unsupported tag: {tag}", Consume().Span);
        }
    }

    private RawXmlDocEntry ParseMember()
    {
        var member = ExpectElement("member");
        string rawDocId = member.Attributes["name"];
        var docId = new CodeReference(rawDocId);
        var entry = new RawXmlDocEntry(docId);

        while (Lookahead is not XmlEndElementNode)
        {
            // If we encounter 'dangling' text or fragments we'll add them to the summary.
            if (TryParseDanglingFragment(entry))
                continue;

            ParseSection(entry);
        }

        ExpectEndElement("member");
        return entry;
    }

    private bool TryParseDanglingFragment(RawXmlDocEntry entry)
    {
        switch (Lookahead)
        {
            case XmlTextNode:
            case XmlElementNode element when _fragmentParsers.ContainsKey(element.Name):
            case XmlEmptyElementNode emptyElement when _fragmentParsers.ContainsKey(emptyElement.Name):
                entry.Summary.Add(NextFragment());
                return true;

            default:
                return false;
        }
    }

    private void ParseSection(RawXmlDocEntry entry)
    {
        if (Lookahead is not XmlElementNode element)
        {
            ThrowHelper.ThrowNodeTypeMismatch<object>(Lookahead, XmlNodeKind.Element, LastNode);
            return;
        }

        if (!_sectionParsers.TryGetValue(element.Name, out var sectionParser))
            throw new XmlDocParserException($"Unexpected section: {element.Name}", element.Span);

        sectionParser.ParseSection(this, entry);
    }
}