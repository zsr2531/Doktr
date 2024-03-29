using System;
using System.Collections;
using Doktr.Core.Models.Collections;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Doktr.Xml.XmlDoc.SectionParsers;
using NSubstitute;
using Serilog;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public abstract class FragmentTests
{
    private readonly ISectionParser[] _sectionParsers = Array.Empty<ISectionParser>();
    private readonly IFragmentParser[] _fragmentParsers;

    protected FragmentTests(IFragmentParser fragmentParser) => _fragmentParsers = new[] { fragmentParser };

    protected DocumentationFragmentCollection ParseXmlDoc(string input)
    {
        var parser = CreateParser(input);
        var map = parser.ParseXmlDoc();
        var entry = Assert.Single(map);
        Assert.False(parser.HasIssues);

        return entry.Value.Summary;
    }

    protected XmlDocParser CreateParser(string input)
    {
        string fullInput = $"<member name='T:Test'>{input}</member>";
        var nodes = new XmlParser(fullInput).ParseXmlNodes();
        var parser = new XmlDocParser(_sectionParsers, _fragmentParsers, Substitute.For<ILogger>(), nodes);

        return parser;
    }

    protected static T AssertSingleChildIsType<T>(IEnumerable fragments)
    {
        object child = Assert.Single(fragments);
        return Assert.IsType<T>(child);
    }
}