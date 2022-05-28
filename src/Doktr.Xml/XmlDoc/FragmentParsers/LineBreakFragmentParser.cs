using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class LineBreakFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "br" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        processor.ExpectEmptyElement(SupportedTags);
        return new LineBreakFragment();
    }
}