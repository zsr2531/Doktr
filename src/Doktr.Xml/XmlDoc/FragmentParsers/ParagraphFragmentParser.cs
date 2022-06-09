using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class ParagraphFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "p", "para" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElement(SupportedTags);
        var paragraph = new ParagraphFragment();
        while (processor.Lookahead.IsNotEndElementOrEof())
            paragraph.Children.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        return paragraph;
    }
}