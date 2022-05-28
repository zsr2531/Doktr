namespace Doktr.Xml.XmlDoc.SectionParsers;

public interface ISectionParser
{
    string Tag { get; }

    void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry);
}