using Doktr.Lifters.Common.XmlDoc.Collections;

namespace Doktr.Lifters.Common.XmlDoc.Xml;

public interface IXmlParser
{
    XmlNodeCollection ParseXmlNodes(string path);
}