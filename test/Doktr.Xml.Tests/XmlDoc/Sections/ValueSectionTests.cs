using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class ValueSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public ValueSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Value()
    {
        const string input = "<value>Hello</value>";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(doc).Value;
        var text = Assert.IsType<TextFragment>(Assert.Single(entry.Value));
        Assert.Equal("Hello", text.Text);
    }
}