using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public interface IFragmentParser
{
    string[] SupportedTags { get; }

    DocumentationFragment ParseFragment(IXmlDocProcessor processor);
}