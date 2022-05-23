using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class ItalicFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "i" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var fragment = new ItalicFragment();
        var start = processor.ExpectElement(SupportedTags);
        while (processor.Lookahead is not XmlEndElementNode)
            fragment.Children.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        return fragment;
    }
}