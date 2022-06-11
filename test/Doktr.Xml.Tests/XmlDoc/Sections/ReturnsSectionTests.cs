using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class ReturnsSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public ReturnsSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Returns()
    {
        const string input = "<returns>Hello</returns>";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(doc).Value;
        var text = Assert.IsType<TextFragment>(Assert.Single(entry.Returns));
        Assert.Equal("Hello", text.Text);
    }
}