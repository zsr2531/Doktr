using System.Collections.ObjectModel;

namespace Doktr.Xml.XmlDoc.Collections;

public class XmlDocDiagnosticCollection : Collection<XmlDocDiagnostic>
{
    public bool IsEmpty => Count == 0;
}