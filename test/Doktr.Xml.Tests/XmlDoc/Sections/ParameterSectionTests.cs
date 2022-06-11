using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class ParameterSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public ParameterSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Parameter()
    {
        const string input = "<param name='test'>Hello</param>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(map).Value;
        (string name, var doc) = Assert.Single(entry.Parameters);
        Assert.Equal("test", name);
        var content = Assert.IsType<TextFragment>(Assert.Single(doc));
        Assert.Equal("Hello", content.Text);
    }

    [Fact]
    public void Missing_Name()
    {
        const string input = "<param>Hello</param>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.True(parser.HasIssues);
        Assert.True(parser.HasErrors);
        var entry = Assert.Single(map).Value;
        Assert.Empty(entry.Parameters);
    }
}