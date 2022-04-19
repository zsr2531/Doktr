using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Fragments;

public enum TableAlignment
{
    Left,
    Center,
    Right
}

public class TableFragment : DocumentationFragment
{
    public TableAlignment Alignment { get; set; } = TableAlignment.Left;
    public RowSegment Header { get; set; } = new();
    public RowSegmentCollection Rows { get; set; } = new();
    
    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitTable(this);

    public override TableFragment Clone() => new()
    {
        Alignment = Alignment,
        Header = Header.Clone(),
        Rows = Rows.Clone()
    };
}