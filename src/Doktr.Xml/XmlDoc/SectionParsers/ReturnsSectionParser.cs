namespace Doktr.Xml.XmlDoc.SectionParsers;

public class ReturnsSectionParser : ISectionParser
{
    public string Tag => "returns";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        var start = processor.ExpectElement(Tag);
        while (processor.Lookahead.IsNotEndElementOrEof())
            entry.Returns.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
    }
}