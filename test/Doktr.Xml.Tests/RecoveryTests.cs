using Xunit;

namespace Doktr.Xml.Tests;

public class RecoveryTests
{
    [Fact]
    public void Incomplete_Prolog()
    {
        var parser = new XmlParser("<?xml version='1.0'><hello>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        Assert.NotEmpty(diagnostics);
        var node = Assert.IsType<XmlElementNode>(Assert.Single(result));
        Assert.Equal("hello", node.Name);
    }

    [Fact]
    public void Incomplete_Attribute_With_Newline()
    {
        var parser = new XmlParser("<hello version='1.0>\n<hello>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        Assert.NotEmpty(diagnostics);
        Assert.Equal(2, result.Count);
        var first = Assert.IsType<XmlTextNode>(result[0]);
        var second = Assert.IsType<XmlElementNode>(result[1]);

        Assert.Equal("\n", first.Text);
        Assert.Equal("hello", second.Name);
    }
}