using System;
using Doktr.Models.Segments;
using Xunit;

namespace Doktr.XmlParserTests;

public class DocumentationSegmentsTest : XmlParserTestBase
{
    [Theory]
    [InlineData("b")]
    [InlineData("strong")]
    public void BoldDocumentationSegment(string tag) =>
        TestSimpleSegment<BoldDocumentationSegment>(tag, b => Assert.Single(b.Content));

    [Theory]
    [InlineData("i")]
    [InlineData("italic")]
    public void ItalicDocumentationSegment(string tag) =>
        TestSimpleSegment<ItalicDocumentationSegment>(tag, i => Assert.Single(i.Content));

    [Theory]
    [InlineData("p")]
    [InlineData("para")]
    public void ParagraphDocumentationSegment(string tag) =>
        TestSimpleSegment<ParagraphDocumentationSegment>(tag, i => Assert.Single(i.Content));

    [Theory]
    [InlineData("c")]
    public void MonospaceDocumentationSegment(string tag) =>
        TestSimpleSegment<MonospaceDocumentationSegment>(tag, m => new TextDocumentationSegment(m.Content));

    [Theory]
    [InlineData("code")]
    public void CodeBlockDocumentationSegment(string tag) =>
        TestSimpleSegment<CodeBlockDocumentationSegment>(tag, c => new TextDocumentationSegment(c.Content));

    private void TestSimpleSegment<T>(string tag, Func<T, IDocumentationSegment> contentFunc)
    {
        const string text = "test";
        string xml = $"<summary><{tag}>{text}</{tag}></summary>";

        var result = ParseSingleXml(xml, out var diagnostics);
        Assert.Empty(diagnostics);

        var element = Assert.Single(result.Summary);
        var simpleSegment = Assert.IsType<T>(element);
        var textSegment = Assert.IsType<TextDocumentationSegment>(contentFunc(simpleSegment));

        Assert.Equal(text, textSegment.Content);
    }
}