using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class ListFragmentTests : FragmentTests
{
    public ListFragmentTests()
        : base(new ListFragmentParser())
    {
    }

    [Theory]
    [InlineData("bullet", ListStyle.Bullet)]
    [InlineData("numbered", ListStyle.Numbered)]
    public void Vanilla_Normal(string style, ListStyle expectedStyle)
    {
        var entry = ParseXmlDoc($"<list type='{style}'><item>Test</item></list>");

        var list = AssertSingleChildIsType<ListFragment>(entry);
        Assert.Equal(expectedStyle, list.Style);

        var item = AssertSingleChildIsType<VanillaListItemFragment>(list.Items);
        var content = AssertSingleChildIsType<TextFragment>(item.Children);
        Assert.Equal("Test", content.Text);
    }

    [Theory]
    [InlineData("bullet", ListStyle.Bullet)]
    [InlineData("numbered", ListStyle.Numbered)]
    public void Vanilla_Description(string style, ListStyle expectedStyle)
    {
        var entry = ParseXmlDoc($"<list type='{style}'><item><description>Test</description></item></list>");

        var list = AssertSingleChildIsType<ListFragment>(entry);
        Assert.Equal(expectedStyle, list.Style);

        var item = AssertSingleChildIsType<VanillaListItemFragment>(list.Items);
        var content = AssertSingleChildIsType<TextFragment>(item.Children);
        Assert.Equal("Test", content.Text);
    }

    [Fact]
    public void Definition()
    {
        var entry = ParseXmlDoc(
            "<list type='bullet'><item><term>Test1</term><description>Test2</description></item></list>");

        var list = AssertSingleChildIsType<ListFragment>(entry);
        var item = AssertSingleChildIsType<DefinitionListItemFragment>(list.Items);
        var term = AssertSingleChildIsType<TextFragment>(item.Term);
        var description = AssertSingleChildIsType<TextFragment>(item.Description);
        Assert.Equal("Test1", term.Text);
        Assert.Equal("Test2", description.Text);
    }

    [Fact]
    public void Mixed_Items()
    {
        var entry = ParseXmlDoc(
            @"<list type='bullet'>
                <item>
                    <term>Test1</term>
                    <description>Test2</description>
                </item>
                <item>Test3</item>
            </list>");

        var list = AssertSingleChildIsType<ListFragment>(entry);
        Assert.Equal(2, list.Items.Count);
        var def = Assert.IsType<DefinitionListItemFragment>(list.Items[0]);
        var van = Assert.IsType<VanillaListItemFragment>(list.Items[1]);

        var term = AssertSingleChildIsType<TextFragment>(def.Term);
        var description = AssertSingleChildIsType<TextFragment>(def.Description);
        Assert.Equal("Test1", term.Text);
        Assert.Equal("Test2", description.Text);

        var content = AssertSingleChildIsType<TextFragment>(van.Children);
        Assert.Equal("Test3", content.Text);
    }

    [Fact]
    public void Invalid_Style()
    {
        var parser = CreateParser("<list type='test'><item>Test</item></list>");
        var entries = parser.ParseXmlDoc();

        Assert.True(parser.HasErrors);
        Assert.False(parser.HasFatalErrors);
        var member = Assert.Single(entries).Value;
        var list = AssertSingleChildIsType<ListFragment>(member.Summary);
        var item = AssertSingleChildIsType<VanillaListItemFragment>(list.Items);
        var content = AssertSingleChildIsType<TextFragment>(item.Children);
        Assert.Equal("Test", content.Text);
    }

    [Fact]
    public void Missing_Style()
    {
        var parser = CreateParser("<list><item>Test</item></list>");
        var entries = parser.ParseXmlDoc();

        Assert.True(parser.HasErrors);
        Assert.False(parser.HasFatalErrors);
        var member = Assert.Single(entries).Value;
        var list = AssertSingleChildIsType<ListFragment>(member.Summary);
        var item = AssertSingleChildIsType<VanillaListItemFragment>(list.Items);
        var content = AssertSingleChildIsType<TextFragment>(item.Children);
        Assert.Equal("Test", content.Text);
    }
}