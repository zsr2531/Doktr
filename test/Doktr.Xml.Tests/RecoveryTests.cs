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
        Assert.False(diagnostics.HasFatal);
        var node = Assert.IsType<XmlElementNode>(Assert.Single(result));
        Assert.Equal("hello", node.Name);
    }
}