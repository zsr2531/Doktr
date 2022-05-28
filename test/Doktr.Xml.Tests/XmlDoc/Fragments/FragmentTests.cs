using System;
using System.Linq;
using Doktr.Core.Models.Collections;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Doktr.Xml.XmlDoc.SectionParsers;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public abstract class FragmentTests
{
    private readonly IFragmentParser[] _fragmentParsers;

    protected FragmentTests(IFragmentParser fragmentParser) => _fragmentParsers = new[] { fragmentParser };

    protected DocumentationFragmentCollection GetSummaryFor(string input)
    {
        var parser = CreateParser(input);
        var map = parser.ParseXmlDoc();
        var entry = map.Single();

        return entry.Value.Summary;
    }

    protected XmlDocParser CreateParser(string input)
    {
        string fullInput = $"<member name='T:Test'>{input}</member>";
        var nodes = new XmlParser(fullInput).ParseXmlNodes();
        var parser = new XmlDocParser(Array.Empty<ISectionParser>(), _fragmentParsers, nodes);

        return parser;
    }

    protected static T AssertSingleChildIsType<T>(DocumentationFragmentCollection fragments)
    {
        var child = Assert.Single(fragments);
        return Assert.IsType<T>(child);
    }
}