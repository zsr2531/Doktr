namespace Doktr.Xml.XmlDoc.SectionParsers;

public class ExampleSectionParser : ISectionParser
{
    public string Tag => "example";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        var start = processor.ExpectElement(Tag);
        while (processor.Lookahead.IsNotEndElementOrEof())
            entry.Example.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
    }
}