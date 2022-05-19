using Antlr4.Runtime;
using Xunit;

namespace Doktr.Xml.Tests;

public class SimpleInputTests
{
    [Fact]
    public void Empty()
    {
        var parser = new XmlParser("");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        Assert.Empty(result);
        var exception = Assert.Single(diagnostics).Exception;
        Assert.IsType<InputMismatchException>(exception);
    }

    [Fact]
    public void Just_Prolog()
    {
        var parser = new XmlParser("<?xml version='1.0'?>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        Assert.Empty(result);
        var exception = Assert.Single(diagnostics).Exception;
        Assert.IsType<InputMismatchException>(exception);
    }

    [Fact]
    public void Prolog_And_EmptyElement()
    {
        var parser = new XmlParser("<?xml version='1.0'?><root/>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        var node = Assert.IsType<XmlEmptyElementNode>(Assert.Single(result));
        Assert.Empty(diagnostics);
        Assert.Equal("root", node.Name);
    }

    [Fact]
    public void Just_EmptyElement()
    {
        var parser = new XmlParser("<root/>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        var node = Assert.IsType<XmlEmptyElementNode>(Assert.Single(result));
        Assert.Empty(diagnostics);
        Assert.Equal("root", node.Name);
    }

    [Fact]
    public void Just_Element()
    {
        var parser = new XmlParser("<root>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        var node = Assert.IsType<XmlElementNode>(Assert.Single(result));
        Assert.Empty(diagnostics);
        Assert.Equal("root", node.Name);
    }

    [Fact]
    public void Just_Element_With_Attributes()
    {
        var parser = new XmlParser("<root attr1='value1' attr2='value2'>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        var node = Assert.IsType<XmlElementNode>(Assert.Single(result));
        var attributes = node.Attributes;
        Assert.Empty(diagnostics);
        Assert.Equal("root", node.Name);
        Assert.Equal("value1", attributes["attr1"]);
        Assert.Equal("value2", attributes["attr2"]);
    }

    [Fact]
    public void Just_EndElement()
    {
        var parser = new XmlParser("</root>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        var node = Assert.IsType<XmlEndElementNode>(Assert.Single(result));
        Assert.Empty(diagnostics);
        Assert.Equal("root", node.Name);
    }

    [Fact]
    public void Just_EmptyElement_With_Attributes()
    {
        var parser = new XmlParser("<root attr1='value1' attr2='value2'/>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        var node = Assert.IsType<XmlEmptyElementNode>(Assert.Single(result));
        var attributes = node.Attributes;
        Assert.Empty(diagnostics);
        Assert.Equal("root", node.Name);
        Assert.Equal("value1", attributes["attr1"]);
        Assert.Equal("value2", attributes["attr2"]);
    }

    [Fact]
    public void Element_And_EndElement()
    {
        var parser = new XmlParser("<root></root>");
        var result = parser.ParseXmlNodes();
        var diagnostics = parser.Diagnostics;

        Assert.Equal(2, result.Count);
        var first = Assert.IsType<XmlElementNode>(result[0]);
        var second = Assert.IsType<XmlEndElementNode>(result[1]);
        Assert.Empty(diagnostics);
        Assert.Equal("root", first.Name);
        Assert.Equal("root", second.Name);
    }
}