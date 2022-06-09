using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class TypeParameterReferenceFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "typeparamref" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectEmptyElement(SupportedTags);
        try
        {
            string name = start.ExpectAttribute("name");
            return new TypeParameterReferenceFragment(name);
        }
        catch (XmlDocParserException ex)
        {
            processor.ReportDiagnostic(XmlDocDiagnostic.MakeWarning(start.Span, ex.Message));
            return new TypeParameterReferenceFragment("?MISSING-NAME?");
        }
    }
}