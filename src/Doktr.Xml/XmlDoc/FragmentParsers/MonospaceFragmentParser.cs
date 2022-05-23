using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class MonospaceFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "c" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var fragment = new MonospaceFragment();
        var start = processor.ExpectElement(SupportedTags);
        while (processor.Lookahead is not XmlEndElementNode)
            fragment.Children.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        return fragment;
    }
}