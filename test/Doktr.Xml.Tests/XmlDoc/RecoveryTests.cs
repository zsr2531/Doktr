using System.Linq;
using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc;

public class RecoveryTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public RecoveryTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void Recover_To_Next_Member()
    {
        var parser = _fixture.CreateParser(
            "Test<invalid>hello",
            "Test2",
            "<see cref=''>"
        );
        var result = parser.ParseXmlDoc();

        Assert.True(parser.HasErrors);
        Assert.Equal(3, result.Count);
        var first = result.First().Value;
        var second = result.Skip(1).First().Value;
        var third = result.Skip(2).First().Value;

        var firstContent = Assert.IsType<TextFragment>(Assert.Single(first.Summary));
        Assert.Equal("Test", firstContent.Text);

        var secondContent = Assert.IsType<TextFragment>(Assert.Single(second.Summary));
        Assert.Equal("Test2", secondContent.Text);

        Assert.Empty(third.Summary);
    }
}