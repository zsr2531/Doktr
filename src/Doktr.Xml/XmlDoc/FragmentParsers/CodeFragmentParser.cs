using System.Text;
using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class CodeFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "code" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var fragment = new CodeFragment();
        var start = processor.ExpectElement(SupportedTags);
        var sb = new StringBuilder();
        while (processor.Lookahead.IsNotEndElementOrNull())
        {
            var text = processor.ExpectText();
            sb.Append(text.Text);
        }

        processor.ExpectEndElement(start.Name);
        return fragment;
    }
}