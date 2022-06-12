using Doktr.Xml.Collections;

namespace Doktr.Xml;

public interface IXmlParser
{
    XmlNodeCollection ParseXmlNodes();

    XmlDiagnosticCollection Diagnostics { get; }
}