using Doktr.Core.Models;
using Doktr.Core.Models.Fragments;

namespace Doktr.Xml.XmlDoc;

public interface IXmlDocProcessor
{
    XmlNode Lookahead { get; }

    DocumentationFragment NextFragment();

    XmlElementNode ExpectElement(params string[] names);

    XmlEndElementNode ExpectEndElement(string name);

    XmlEmptyElementNode ExpectEmptyElement(params string[] names);

    XmlTextNode ExpectText();

    XmlComplexNode ExpectElementOrEmptyElement(params string[] names);

    XmlNode Consume();

    void ReportDiagnostic(XmlDocDiagnostic diagnostic);

    CodeReference ParseCodeReference(XmlComplexNode node, string key = "cref");
}