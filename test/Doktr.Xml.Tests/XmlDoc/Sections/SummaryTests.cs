using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class SummaryTests : IClassFixture<XmlDocFixture>
{
    private readonly XmlDocFixture _fixture;

    public SummaryTests(XmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Summary()
    {
        const string input = "<summary>Hello</summary>";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        var entry = Assert.Single(doc).Value;
        var element = Assert.IsType<TextFragment>(Assert.Single(entry.Summary));

        Assert.Equal("Hello", element.Text);
    }
}