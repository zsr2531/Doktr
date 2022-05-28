using Doktr.Core.Models;
using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;

namespace Doktr.Xml.XmlDoc;

public partial class XmlDocParser
{
    private const string Inheritdoc = "inheritdoc";
    private const string Member = "member";
    private const string Name = "name";
    private const string Cref = "cref";

    public DocumentationFragment NextFragment()
    {
        return Lookahead switch
        {
            XmlTextNode => new TextFragment(Consume<XmlTextNode>().Text),
            IHasNameAndAttributes { Name: { } s } => GetFragmentParser(s).ParseFragment(this),
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
        var member = ExpectElement(Member);
        if (!member.Attributes.TryGetValue(Name, out string? rawDocId))
            throw new XmlDocParserException("Expected a name attribute on member tag, but found nothing", member.Span);

        var docId = new CodeReference(rawDocId);
        var entry = new RawXmlDocEntry(docId);

        while (Lookahead is not XmlEndElementNode)
        {
            // If we encounter 'dangling' text or fragments we'll add them to the summary.
            if (TryParseDanglingFragment(entry))
                continue;

            ParseSection(entry);
        }

        ExpectEndElement(Member);
        return entry;
    }

    private bool TryParseDanglingFragment(RawXmlDocEntry entry)
    {
        switch (Lookahead)
        {
            case XmlTextNode:
            case IHasNameAndAttributes element when _fragmentParsers.ContainsKey(element.Name):
                entry.Summary.Add(NextFragment());
                return true;

            // TODO: Warn user if entry already inherits documentation? Throwing here is excessive.
            case XmlEmptyElementNode { Name: Inheritdoc } emptyElement when !entry.InheritsDocumentation:
                if (emptyElement.Attributes.TryGetValue(Cref, out string? from))
                    entry.InheritsDocumentationExplicitlyFrom = new CodeReference(from);
                else
                    entry.InheritsDocumentationImplicitly = true;
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