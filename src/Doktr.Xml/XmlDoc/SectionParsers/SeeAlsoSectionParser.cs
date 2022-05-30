using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.SectionParsers;

public class SeeAlsoSectionParser : ISectionParser
{
    private const string Cref = "cref";
    private const string Href = "href";

    public string Tag => "seealso";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        var start = processor.ExpectElementOrEmptyElement(Tag);
        var attributes = ((IHasNameAndAttributes) start).Attributes;
        var replacement = start.Kind == XmlNodeKind.Element ? ParseReplacement(processor) : null;
        if (replacement is not null)
            processor.ExpectEndElement(Tag);

        if (attributes.TryGetValue(Cref, out string? cref))
            ParseCodeReference(cref, replacement, entry);
        else if (attributes.TryGetValue(Href, out string? href))
            ParseLinkReference(href, replacement, entry);
        else
            throw new XmlDocParserException("Expected a 'cref' or an 'href' attribute, but found neither.", start.Span);
    }

    private static void ParseCodeReference(
        string cref,
        DocumentationFragmentCollection? replacement,
        RawXmlDocEntry entry)
    {
        var reference = new CodeReference(cref);
        var fragment = new CodeReferenceFragment(reference)
        {
            Replacement = replacement
        };

        entry.SeeAlso.Add(fragment);
    }

    private static void ParseLinkReference(
        string href,
        DocumentationFragmentCollection? replacement,
        RawXmlDocEntry entry)
    {
        var fragment = new LinkReferenceFragment(href)
        {
            Replacement = replacement
        };

        entry.SeeAlso.Add(fragment);
    }

    private static DocumentationFragmentCollection ParseReplacement(IXmlDocProcessor processor)
    {
        var replacement = new DocumentationFragmentCollection();
        while (processor.Lookahead.IsNotEndElementOrEof())
            replacement.Add(processor.NextFragment());

        return replacement;
    }
}