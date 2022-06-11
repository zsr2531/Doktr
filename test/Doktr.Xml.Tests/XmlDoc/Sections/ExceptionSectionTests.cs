using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class ExceptionSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public ExceptionSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Exception()
    {
        const string input = "<exception cref='T:Test'>Hello</exception>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(map).Value;
        var (codeRef, doc) = Assert.Single(entry.Exceptions);
        Assert.Equal("T:Test", codeRef.Identifier);
        var content = Assert.IsType<TextFragment>(Assert.Single(doc));
        Assert.Equal("Hello", content.Text);
    }

    [Fact]
    public void Missing_CodeRef()
    {
        const string input = "<exception>Hello</exception>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.True(parser.HasIssues);
        Assert.True(parser.HasErrors);
        var entry = Assert.Single(map).Value;
        Assert.Empty(entry.Exceptions);
    }

    [Fact]
    public void Invalid_CodeRef()
    {
        const string input = "<exception cref='Invalid'>Hello</exception>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.True(parser.HasIssues);
        Assert.True(parser.HasErrors);
        var entry = Assert.Single(map).Value;
        Assert.Empty(entry.Exceptions);
    }
}