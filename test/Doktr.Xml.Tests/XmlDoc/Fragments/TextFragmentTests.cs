using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class TextFragmentTests : IClassFixture<XmlDocFixture>
{
    private readonly XmlDocFixture _fixture;

    public TextFragmentTests(XmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Dangling_Text()
    {
        const string input = "Hello";
        var parser = _fixture.CreateParser(input);
        var doc = parser.ParseXmlDoc();

        var entry = Assert.Single(doc).Value;
        var text = Assert.IsType<TextFragment>(Assert.Single(entry.Summary));
        Assert.Equal("Hello", text.Text);
    }
}