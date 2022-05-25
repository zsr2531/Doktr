using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class SummaryTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public SummaryTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Summary()
    {
        const string input = "<summary>Hello</summary>";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        var entry = Assert.Single(doc).Value;
        var text = Assert.IsType<TextFragment>(Assert.Single(entry.Summary));
        Assert.Equal("Hello", text.Text);
    }

    [Fact]
    public void Dangling_Text_Should_Be_Added_To_Summary()
    {
        const string input = "Hello";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        var entry = Assert.Single(doc).Value;
        var text = Assert.IsType<TextFragment>(Assert.Single(entry.Summary));
        Assert.Equal("Hello", text.Text);
    }

    [Fact]
    public void Dangling_Element_Should_Be_Added_To_Summary()
    {
        const string input = "<b>Hello</b>";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        var entry = Assert.Single(doc).Value;
        var bold = Assert.IsType<BoldFragment>(Assert.Single(entry.Summary));
        var text = Assert.IsType<TextFragment>(Assert.Single(bold.Children));
        Assert.Equal("Hello", text.Text);
    }
}