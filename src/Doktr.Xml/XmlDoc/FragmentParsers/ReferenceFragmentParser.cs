using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Fragments;
using Doktr.Xml.Collections;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class ReferenceFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "see" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElementOrEmptyElement(SupportedTags);
        (string name, var attributes) = ExtractData(start);
        var replacement = start.Kind == XmlNodeKind.Element ? ParseReplacementText(processor) : null;
        if (replacement is not null)
            processor.ExpectEndElement(name);

        if (attributes.TryGetValue("cref", out string? cref))
            return ParseCodeReference(cref, replacement);
        if (attributes.TryGetValue("href", out string? href))
            return ParseLinkReference(href, replacement);

        throw new XmlDocParserException("Expected a 'cref' or an 'href' attribute, but found neither.", start.Span);
    }

    private static CodeReferenceFragment ParseCodeReference(string cref, DocumentationFragmentCollection? replacement)
    {
        var reference = new CodeReference(cref);
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

    private static DocumentationFragmentCollection ParseReplacementText(IXmlDocProcessor processor)
    {
        var replacement = new DocumentationFragmentCollection();
        while (processor.Lookahead is not XmlEndElementNode)
            replacement.Add(processor.NextFragment());

        return replacement;
    }

    private static (string, XmlAttributeMap) ExtractData(XmlNode node)
    {
        return node switch
        {
            XmlElementNode element => (element.Name, element.Attributes),
            XmlEmptyElementNode emptyElement => (emptyElement.Name, emptyElement.Attributes),
            _ => throw new ArgumentOutOfRangeException(nameof(node))
        };
    }
}