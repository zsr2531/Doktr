using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class UnderlineFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "u" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var fragment = new UnderlineFragment();
        var start = processor.ExpectElement(SupportedTags);
        while (processor.Lookahead is not XmlEndElementNode)
            fragment.Children.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        return fragment;
    }
}