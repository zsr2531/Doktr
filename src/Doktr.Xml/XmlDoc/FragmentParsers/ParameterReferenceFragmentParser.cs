using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class ParameterReferenceFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "paramref" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectEmptyElement(SupportedTags);
        string name = start.ExpectAttribute("name");

        return new ParameterReferenceFragment(name);
    }
}