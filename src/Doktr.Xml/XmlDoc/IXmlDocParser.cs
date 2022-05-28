using Doktr.Xml.XmlDoc.Collections;

namespace Doktr.Xml.XmlDoc;

public interface IXmlDocParser : IXmlDocProcessor
{
    RawXmlDocEntryMap ParseXmlDoc();
}