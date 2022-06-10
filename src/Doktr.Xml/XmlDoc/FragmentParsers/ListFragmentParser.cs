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
        if (!start.TryGetAttribute("type", out string? type))
        {
            var diagnostic = XmlDocDiagnostic.MakeWarning(start.Span, "Missing list type, assuming bullet");
            processor.ReportDiagnostic(diagnostic);
            type = "bullet";
        }

        var fragment = type == "table"
            ? (DocumentationFragment) ParseTable(processor)
            : ParseList(processor, start, type);

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