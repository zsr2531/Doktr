using System.Collections.ObjectModel;

namespace Doktr.Xml.Collections;

public class XmlNodeCollection : Collection<XmlNode>
{
    public bool IsEmpty => Count == 0;
}