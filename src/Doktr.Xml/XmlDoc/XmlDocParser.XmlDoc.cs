using Doktr.Core.Models;
using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.Collections;
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
            XmlComplexNode { Name: { } s } => GetFragmentParser(s).ParseFragment(this),
            _ => throw new XmlDocParserException($"Unexpected node: {Lookahead}", Consume().Span)
        };

        IFragmentParser GetFragmentParser(string tag)
        {
            if (_fragmentParsers.TryGetValue(tag, out var fragmentParser))
                return fragmentParser;

            throw new XmlDocParserException($"Unsupported tag: {tag}", Consume().Span);
        }
    }

    public CodeReference ParseCodeReference(XmlComplexNode member, string key = Cref)
    {
        try
        {
            string rawDocId = member.ExpectAttribute(key);
            var docId = new CodeReference(rawDocId);
            _current = docId;
            return docId;
        }
        catch (ArgumentException ex)
        {
            throw new XmlDocParserException(ex.Message[..^1], member.Span);
        }
    }


    private void ParseMember(RawXmlDocEntryMap map)
    {
        var member = ExpectElement(Member);
        var docId = ParseCodeReference(member, Name);
        var entry = new RawXmlDocEntry(docId);
        map[docId] = entry;

        while (Lookahead is not XmlEndElementNode)
        {
            // If we encounter 'dangling' text or fragments we'll add them to the summary.
            if (TryParseDanglingFragment(entry))
                continue;

            ParseSection(entry);
        }

        ExpectEndElement(Member);
    }

    private bool TryParseDanglingFragment(RawXmlDocEntry entry)
    {
        switch (Lookahead)
        {
            case XmlTextNode:
            case XmlComplexNode element when _fragmentParsers.ContainsKey(element.Name):
                entry.Summary.Add(NextFragment());
                return true;

            case XmlEmptyElementNode { Name: Inheritdoc } emptyElement:
                var node = Consume();
                if (entry.InheritsDocumentation)
                {
                    ReportDiagnostic(XmlDocDiagnostic.MakeWarning(node.Span,
                        "Multiple inheritdoc tags found, using latest one"));
                }

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
        if (Lookahead is not XmlComplexNode element)
        {
            ThrowHelper.ThrowNodeTypeMismatch<object>(Lookahead, XmlNodeKind.Element, XmlNodeKind.EmptyElement);
            return;
        }

        if (!_sectionParsers.TryGetValue(element.Name, out var sectionParser))
            throw new XmlDocParserException($"Unexpected section: {element.Name}", element.Span);

        sectionParser.ParseSection(this, entry);
    }
}