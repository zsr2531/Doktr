namespace Doktr.Xml.XmlDoc.SectionParsers;

public class SummarySectionParser : ISectionParser
{
    public string Tag => "summary";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        processor.ExpectElement(Tag);
        while (processor.Lookahead.IsNotEndElementOrNull())
            entry.Summary.Add(processor.NextFragment());

        processor.ExpectEndElement(Tag);
    }
}