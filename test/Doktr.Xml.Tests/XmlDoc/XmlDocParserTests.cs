using Doktr.Core.Models.Fragments;
using Doktr.Xml.Collections;
using Doktr.Xml.XmlDoc;
using NSubstitute;
using Serilog;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc;

public class XmlDocParserTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public XmlDocParserTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Single_Member()
    {
        const string input = "<member name='T:Test'>Hello</member>";
        var parser = new XmlDocParser(
            _fixture.Sections, _fixture.Fragments, Substitute.For<ILogger>(), ParseInput(input));
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
        var parser = new XmlDocParser(
            _fixture.Sections, _fixture.Fragments, Substitute.For<ILogger>(), ParseInput(input));
        var result = parser.ParseXmlDoc();

        Assert.Empty(result);
        Assert.NotEmpty(parser.Diagnostics);
        Assert.True(parser.HasFatalErrors);
    }

    private static XmlNodeCollection ParseInput(string input)
    {
        var parser = new XmlParser(input);
        return parser.ParseXmlNodes();
    }
}