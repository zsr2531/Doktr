using Doktr.Core.Models.Fragments;
using Doktr.Core.Models.Segments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public partial class ListFragmentParser
{
    private const string ListHeader = "listheader";

    // TODO: Parse the alignment for the table... somehow.
    private static TableFragment ParseTable(IXmlDocProcessor processor)
    {
        var table = new TableFragment();
        var header = ParseTableHeader(processor);
        int columns = header.Columns.Count;
        while (processor.Lookahead.IsNotEndElementOrNull())
        {
            // TODO: Warn if the number of columns in the header doesn't match the number of columns in the rows.
            var row = ParseRow(processor, columns);
            table.Rows.Add(row);
        }

        return table;
    }

    private static RowSegment ParseTableHeader(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElement(ListHeader);
        var row = ParseRow(processor);

        processor.ExpectEndElement(start.Name);
        return row;
    }

    private static RowSegment ParseRow(IXmlDocProcessor processor, int maxCount = -1)
    {
        var row = new RowSegment();
        while ((maxCount == -1 || --maxCount > 0) && processor.Lookahead.IsNotEndElementOrNull())
            row.Columns.Add(ParseColumn(processor));

        return row;
    }

    private static ColumnSegment ParseColumn(IXmlDocProcessor processor)
    {
        var term = ParseTerm(processor);

        return new ColumnSegment
        {
            Content = term
        };
    }
}