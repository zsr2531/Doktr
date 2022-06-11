using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class RemarksSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public RemarksSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Remarks()
    {
        const string input = "<remarks>Hello</remarks>";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(doc).Value;
        var text = Assert.IsType<TextFragment>(Assert.Single(entry.Remarks));
        Assert.Equal("Hello", text.Text);
    }
}