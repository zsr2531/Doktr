using System.Collections.ObjectModel;

namespace Doktr.Xml.Collections;

public class XmlDiagnosticCollection : Collection<XmlDiagnostic>
{
    public bool IsEmpty => Count == 0;
}