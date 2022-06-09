namespace Doktr.Xml.XmlDoc.SectionParsers;

public class ValueSectionParser : ISectionParser
{
    public string Tag => "value";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        var start = processor.ExpectElement(Tag);
        while (processor.Lookahead.IsNotEndElementOrEof())
            entry.Value.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
    }
}