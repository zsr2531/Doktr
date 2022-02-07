using System.Xml;
using Doktr.Models.Segments;
using Xunit;

namespace Doktr.XmlParserTests;

public class TrivialCasesTest : XmlParserTestBase
{
    [Fact]
    public void EmptyEntry()
    {
        var result = ParseSingleXml("", out var diagnostics);
        Assert.Empty(diagnostics);
        
        Assert.Null(result.InheritFrom);
        Assert.Empty(result.Summary);
        Assert.Empty(result.Parameters);
        Assert.Empty(result.TypeParameters);
        Assert.Empty(result.Exceptions);
        Assert.Empty(result.Returns);
        Assert.Empty(result.Examples);
        Assert.Empty(result.Remarks);
        Assert.Empty(result.Seealso);
    }

    [Fact]
    public void UnmatchedTag()
    {
        Assert.Throws<XmlException>(() => ParseSingleXml("<b>test", out _));
    }

    [Fact]
    public void StandaloneTextAddedToSummary()
    {
        const string text = "test";
        
        var result = ParseSingleXml(text, out var diagnostics);
        Assert.Empty(diagnostics);
        
        var element = Assert.Single(result.Summary);
        var textSegment = Assert.IsType<TextDocumentationSegment>(element);
        Assert.Equal(text, textSegment.Content);
    }
}