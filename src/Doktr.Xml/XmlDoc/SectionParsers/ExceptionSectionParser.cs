using Doktr.Core.Models;
using Doktr.Core.Models.Collections;

namespace Doktr.Xml.XmlDoc.SectionParsers;

public class ExceptionSectionParser : ISectionParser
{
    public string Tag => "exception";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        var start = processor.ExpectElement(Tag);
        var ex = processor.ParseCodeReference(start);
        var documentation = new DocumentationFragmentCollection();
        while (processor.Lookahead.IsNotEndElementOrEof())
            documentation.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        entry.Exceptions.Add(ex, documentation);
    }
}