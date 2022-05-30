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

    XmlNode ExpectElementOrEmptyElement(params string[] names);

    XmlNode Consume();

    void ReportDiagnostic(XmlDocDiagnostic diagnostic);
}