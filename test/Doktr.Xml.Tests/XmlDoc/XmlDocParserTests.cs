using Doktr.Core.Models.Fragments;
using Doktr.Xml.Collections;
using Doktr.Xml.XmlDoc;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc;

public class XmlDocParserTests : IClassFixture<XmlDocFixture>
{
    private readonly XmlDocFixture _fixture;

    public XmlDocParserTests(XmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Single_Member()
    {
        const string input = "<member name='T:Test'>Hello</member>";
        var parser = new XmlDocParser(_fixture.Sections, _fixture.Fragments, ParseInput(input));
        var doc = parser.ParseXmlDoc();

        var member = Assert.Single(doc);
        Assert.Equal("T:Test", member.Key.Identifier);
        var text = Assert.IsType<TextFragment>(Assert.Single(member.Value.Summary));
        Assert.Equal("Hello", text.Text);
    }

    [Fact]
    public void Missing_Name()
    {
        const string input = "<member>Hello</member>";
        var parser = new XmlDocParser(_fixture.Sections, _fixture.Fragments, ParseInput(input));
        var doc = parser.ParseXmlDoc();

        Assert.Empty(doc);
    }

    private static XmlNodeCollection ParseInput(string input)
    {
        var parser = new XmlParser(input);
        return parser.ParseXmlNodes();
    }
}