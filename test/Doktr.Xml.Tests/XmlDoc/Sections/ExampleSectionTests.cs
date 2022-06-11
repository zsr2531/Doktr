using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class ExampleSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public ExampleSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Example()
    {
        const string input = "<example>Hello</example>";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(doc).Value;
        var text = Assert.IsType<TextFragment>(Assert.Single(entry.Example));
        Assert.Equal("Hello", text.Text);
    }
}