using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class CodeFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "code" };

    // TODO: Maybe warn for non-text content?
    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var fragment = new CodeFragment();
        var start = processor.ExpectElement(SupportedTags);
        // while (processor.Lookahead.IsNotEndElementOrNull())
        // {
        //     var text = processor.ExpectText();
        //     sb.Append(text.Text);
        // }
        while (processor.Lookahead.IsNotEndElementOrEof())
            fragment.Content.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        return fragment;
    }
}