using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public partial class ListFragmentParser : IFragmentParser
{
    private const string Item = "item";
    private const string Term = "term";
    private const string Description = "description";

    public string[] SupportedTags { get; } = { "list" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElement(SupportedTags);
        string type = start.ExpectAttribute("type");
        var fragment = type == "table"
            ? (DocumentationFragment) ParseTable(processor)
            : ParseList(processor, type);

        processor.ExpectEndElement(start.Name);
        return fragment;
    }

    private static DocumentationFragmentCollection ParseTerm(IXmlDocProcessor processor) =>
        ParseEncapsulatedFragments(processor, Term);

    private static DocumentationFragmentCollection ParseDescription(IXmlDocProcessor processor) =>
        ParseEncapsulatedFragments(processor, Description);

    private static DocumentationFragmentCollection ParseEncapsulatedFragments(IXmlDocProcessor processor, string name)
    {
        var start = processor.ExpectElement(name);
        var fragments = new DocumentationFragmentCollection();

        while (processor.Lookahead.IsNotEndElementOrEof())
            fragments.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        return fragments;
    }
}