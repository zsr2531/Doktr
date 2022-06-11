using Doktr.Core.Models.Fragments;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Sections;

public class SeeAlsoSectionTests : IClassFixture<SimpleXmlDocFixture>
{
    private readonly SimpleXmlDocFixture _fixture;

    public SeeAlsoSectionTests(SimpleXmlDocFixture fixture) => _fixture = fixture;

    [Fact]
    public void SeeAlso()
    {
        const string input = "<seealso cref='T:Test'/>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(map).Value;
        var codeRef = Assert.IsType<CodeReferenceFragment>(Assert.Single(entry.SeeAlso));
        Assert.Equal("T:Test", codeRef.CodeReference.Identifier);
        Assert.Null(codeRef.Replacement);
    }

    [Fact]
    public void SeeAlso_With_Replacement()
    {
        const string input = "<seealso cref='T:Test'>Hello</seealso>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.False(parser.HasIssues);
        var entry = Assert.Single(map).Value;
        var codeRef = Assert.IsType<CodeReferenceFragment>(Assert.Single(entry.SeeAlso));
        Assert.Equal("T:Test", codeRef.CodeReference.Identifier);
        Assert.NotNull(codeRef.Replacement);
        var replacement = Assert.IsType<TextFragment>(Assert.Single(codeRef.Replacement!));
        Assert.Equal("Hello", replacement.Text);
    }

    [Fact]
    public void Invalid_CodeRef()
    {
        const string input = "<seealso cref='Invalid'>";
        var parser = _fixture.CreateParser(input);
        var map = parser.ParseXmlDoc();

        Assert.True(parser.HasIssues);
        Assert.True(parser.HasErrors);
        var entry = Assert.Single(map).Value;
        Assert.Empty(entry.SeeAlso);
    }
}