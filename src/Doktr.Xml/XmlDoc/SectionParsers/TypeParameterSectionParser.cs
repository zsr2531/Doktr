using Doktr.Core.Models.Collections;

namespace Doktr.Xml.XmlDoc.SectionParsers;

public class TypeParameterSectionParser : ISectionParser
{
    public string Tag => "typeparam";

    public void ParseSection(IXmlDocProcessor processor, RawXmlDocEntry entry)
    {
        var start = processor.ExpectElement(Tag);
        string typeParamName = start.ExpectAttribute("name");
        var documentation = new DocumentationFragmentCollection();
        while (processor.Lookahead.IsNotEndElementOrNull())
            documentation.Add(processor.NextFragment());

        processor.ExpectEndElement(start.Name);
        entry.TypeParameters.Add(typeParamName, documentation);
    }
}