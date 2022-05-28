using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public class TypeParameterReferenceFragmentParser : IFragmentParser
{
    public string[] SupportedTags { get; } = { "typeparamref" };

    public DocumentationFragment ParseFragment(IXmlDocProcessor processor)
    {
        var start = processor.ExpectEmptyElement(SupportedTags);
        string name = start.ExpectAttribute("name");

        return new TypeParameterReferenceFragment(name);
    }
}