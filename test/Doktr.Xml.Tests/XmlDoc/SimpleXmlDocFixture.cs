using System.Collections.Generic;
using System.Linq;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Doktr.Xml.XmlDoc.SectionParsers;
using NSubstitute;
using Serilog;

namespace Doktr.Xml.Tests.XmlDoc;

public class SimpleXmlDocFixture
{
    private static readonly IEnumerable<IFragmentParser> FragmentParsers;
    private static readonly IEnumerable<ISectionParser> SectionParsers;

    static SimpleXmlDocFixture()
    {
        FragmentParsers = new IFragmentParser[]
        {
            new BoldFragmentParser(),
            new ItalicFragmentParser(),
            new UnderlineFragmentParser(),
            new MonospaceFragmentParser()
        };

        SectionParsers = new ISectionParser[]
        {
            new SummarySectionParser(),
            new RemarksSectionParser()
        };
    }

    public IEnumerable<IFragmentParser> Fragments => FragmentParsers;
    public IEnumerable<ISectionParser> Sections => SectionParsers;

    public XmlDocParser CreateParser(params string[] inputs)
    {
        var transformed = inputs.Select((s, i) => $"<member name='T:Test{i}'>{s}</member>");
        string input = string.Join('\n', transformed);
        var nodes = new XmlParser(input).ParseXmlNodes();
        var doc = new XmlDocParser(Sections, Fragments, Substitute.For<ILogger>(), nodes);

        return doc;
    }
}