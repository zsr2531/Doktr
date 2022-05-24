using System;
using System.Collections.Generic;
using System.Linq;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Doktr.Xml.XmlDoc.SectionParsers;

namespace Doktr.Xml.Tests.XmlDoc;

public class XmlDocFixture
{
    private static readonly IEnumerable<IFragmentParser> FragmentParsers;
    private static readonly IEnumerable<ISectionParser> SectionParsers;

    static XmlDocFixture()
    {
        var types = typeof(XmlDocParser).Assembly.GetTypes();
        var fragments = types.Where(t => t.IsClass && t.IsAssignableTo(typeof(IFragmentParser)));
        var sections = types.Where(t => t.IsClass && t.IsAssignableTo(typeof(ISectionParser)));

        FragmentParsers = fragments.Select(t => (IFragmentParser) Activator.CreateInstance(t)!);
        SectionParsers = sections.Select(t => (ISectionParser) Activator.CreateInstance(t)!);
    }

    public IEnumerable<IFragmentParser> Fragments => FragmentParsers;
    public IEnumerable<ISectionParser> Sections => SectionParsers;

    public XmlDocParser CreateParser(params string[] inputs)
    {
        var transformed = inputs.Select((s, i) => $"<member name='T:Test{i}'>{s}</member>");
        string input = string.Join('\n', transformed);
        var nodes = new XmlParser(input).ParseXmlNodes();
        var doc = new XmlDocParser(Sections, Fragments, nodes);

        return doc;
    }
}