using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class TypeParameterSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public TypeParameterSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void TypeParameter()
    {
        const string input = "<typeparam name='test'>Hello</typeparam>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(map).Value;
        (string name, var doc) = Assert.Single(entry.TypeParameters);
        Assert.Equal("test", name);
        var content = Assert.IsType<TextFragment>(Assert.Single(doc));
        Assert.Equal("Hello", content.Text);
    }

    [Fact]
    public void Missing_Name()
    {
        const string input = "<typeparam>Hello</typeparam>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.True(parser.HasIssues);
        Assert.True(parser.HasErrors);
        var entry = Assert.Single(map).Value;
        Assert.Empty(entry.TypeParameters);
    }
}