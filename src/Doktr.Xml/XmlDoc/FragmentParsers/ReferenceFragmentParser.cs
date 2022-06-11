using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class ReferenceFragmentParser : IFragmentParser
{
    private const string Cref = "cref";
    private const string Href = "href";

    public string[] SupportedTags { get; } = { "see" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElementOrEmptyElement(SupportedTags);
        string name = start.Name;
        var attributes = start.Attributes;
        var replacement = start.Kind == XmlNodeKind.Element ? ParseReplacement(processor) : null;
        if (replacement is not null)
            processor.ExpectEndElement(name);

        if (attributes.TryGetValue(Href, out string? href))
            return ParseLinkReference(href, replacement);
        if (attributes.ContainsKey(Cref))
            return ParseCodeReference(processor, start, replacement);

        throw new XmlDocParserException("Expected a 'cref' or an 'href' attribute, but found neither.", start.Span);
    }

    private static CodeReferenceFragment ParseCodeReference(
        IXmlDocProcessor processor,
        XmlComplexNode node,
        DocumentationFragmentCollection? replacement)
    {
        var reference = processor.ParseCodeReference(node);
        return new CodeReferenceFragment(reference)
        {
            Replacement = replacement
        };
    }

    private static LinkReferenceFragment ParseLinkReference(string href, DocumentationFragmentCollection? replacement)
    {
        return new LinkReferenceFragment(href)
        {
            Replacement = replacement
        };
    }

    private static DocumentationFragmentCollection ParseReplacement(IXmlDocProcessor processor)
    {
        var replacement = new DocumentationFragmentCollection();
        while (processor.Lookahead.IsNotEndElementOrEof())
            replacement.Add(processor.NextFragment());

        return replacement;
    }
}