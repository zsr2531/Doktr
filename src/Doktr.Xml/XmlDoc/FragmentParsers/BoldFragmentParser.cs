using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class BoldFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } =
    {
        "b",
        "bold",
        "em"
    };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var fragment = new BoldFragment();
        var start = processor.ExpectElement(SupportedTags);
        while (processor.Lookahead is not XmlEndElementNode)
            fragment.Children.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        return fragment;
    }
}