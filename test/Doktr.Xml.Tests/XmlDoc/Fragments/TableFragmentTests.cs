using Doktr.Core.Models.Fragments;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Xunit;

namespace Doktr.Xml.Tests.XmlDoc.Fragments;

public class TableFragmentTests : FragmentTests
{
    public TableFragmentTests()
        : base(new ListFragmentParser())
    {
    }

    [Fact]
    public void Table()
    {
        var entry = ParseXmlDoc(@"
            <list type='table'>
                <listheader>
                    <term>First</term>
                    <term>Second</term>
                </listheader>
                <item>
                    <term>Test1</term>
                    <term>Test2</term>
                </item>
            </list>
        ");

        var table = AssertSingleChildIsType<TableFragment>(entry);
        Assert.Equal(2, table.Header.Columns.Count);
        var firstColumn = AssertSingleChildIsType<TextFragment>(table.Header.Columns[0].Content);
        var secondColumn = AssertSingleChildIsType<TextFragment>(table.Header.Columns[1].Content);
        Assert.Equal("First", firstColumn.Text);
        Assert.Equal("Second", secondColumn.Text);

        Assert.Equal(2, table.Header.Columns.Count);
        var firstItem = AssertSingleChildIsType<TextFragment>(table.Rows[0].Columns[0].Content);
        var secondItem = AssertSingleChildIsType<TextFragment>(table.Rows[0].Columns[1].Content);
        Assert.Equal("Test1", firstItem.Text);
        Assert.Equal("Test2", secondItem.Text);
    }

    [Fact]
    public void Fewer_Header_Columns()
    {
        var parser = CreateParser(@"
            <list type='table'>
                <listheader>
                    <term>First</term>
                </listheader>
                <item>
                    <term>Test1</term>
                    <term>Test2</term>
                </item>
            </list>
        ");
        var entries = parser.ParseXmlDoc();
        Assert.True(parser.HasIssues);
        Assert.False(parser.HasErrors);
        var entry = Assert.Single(entries).Value.Summary;

        var table = AssertSingleChildIsType<TableFragment>(entry);
        var headerColumns = table.Header.Columns;
        Assert.Equal(2, headerColumns.Count);
        var first = AssertSingleChildIsType<TextFragment>(headerColumns[0].Content);
        Assert.Equal("First", first.Text);
        Assert.Empty(headerColumns[1].Content);
    }

    [Fact]
    public void Fewer_Entry_Columns()
    {
        var parser = CreateParser(@"
            <list type='table'>
                <listheader>
                    <term>First</term>
                    <term>Second</term>
                </listheader>
                <item>
                    <term>Test1</term>
                </item>
            </list>
        ");
        var entries = parser.ParseXmlDoc();
        Assert.True(parser.HasIssues);
        Assert.False(parser.HasErrors);
        var entry = Assert.Single(entries).Value.Summary;

        var table = AssertSingleChildIsType<TableFragment>(entry);
        var row = Assert.Single(table.Rows);
        var column = Assert.Single(row.Columns);
        var content = AssertSingleChildIsType<TextFragment>(column.Content);
        Assert.Equal("Test1", content.Text);
    }
}