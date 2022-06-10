using Doktr.Core.Models.Fragments;
using Doktr.Core.Models.Segments;

namespace Doktr.Xml.XmlDoc.FragmentParsers;

public partial class ListFragmentParser
{
    private const string ListHeader = "listheader";

    // TODO: Parse the alignment for the table... somehow.
    private static TableFragment ParseTable(IXmlDocProcessor processor)
    {
        var table = new TableFragment
        {
            Header = ParseTableHeader(processor)
        };
        var header = table.Header;
        int columns = header.Columns.Count;
        while (processor.Lookahead.IsNotEndElementOrEof())
        {
            var start = processor.ExpectElement(Item);
            var row = ParseRow(processor);
            var end = processor.ExpectEndElement(Item);
            int entries = row.Columns.Count;
            if (entries != columns)
            {
                string message = $"Incorrect number of columns (header has {columns}, while the row has {entries})";
                var diagnostic = XmlDocDiagnostic.MakeWarning(start.Span.CombineWith(end.Span), message);
                processor.ReportDiagnostic(diagnostic);
                AdjustHeader(entries);
            }

            table.Rows.Add(row);
        }

        return table;

        void AdjustHeader(int minimum)
        {
            int actualColumns = header.Columns.Count;
            if (actualColumns >= minimum)
                return;

            int difference = minimum - actualColumns;
            for (int i = 0; i < difference; i++)
                header.Columns.Add(new ColumnSegment());
        }
    }

    private static RowSegment ParseTableHeader(IXmlDocProcessor processor)
    {
        var start = processor.ExpectElement(ListHeader);
        var row = ParseRow(processor);

        processor.ExpectEndElement(start.Name);
        return row;
    }

    private static RowSegment ParseRow(IXmlDocProcessor processor)
    {
        var row = new RowSegment();
        while (processor.Lookahead.IsNotEndElementOrEof())
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