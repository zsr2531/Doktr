using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class LineBreakFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "br" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElementOrEmptyElement(SupportedTags);
        if (start.Kind == XmlNodeKind.Element)
            processor.ExpectEndElement(((XmlElementNode) start).Name);

        return new LineBreakFragment();
    }
}