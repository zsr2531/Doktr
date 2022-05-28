using Doktr.Core.Models.Collections;

namespace Doktr.Xml.XmlDoc.SectionParsers;

public class ParameterSectionParser : ISectionParser
{
    public string Tag => "param";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        var start = processor.ExpectElement(Tag);
        string typeParamName = start.ExpectAttribute("name");
        var documentation = new DocumentationFragmentCollection();
        while (processor.Lookahead.IsNotEndElementOrNull())
            documentation.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        entry.Parameters.Add(typeParamName, documentation);
    }
}