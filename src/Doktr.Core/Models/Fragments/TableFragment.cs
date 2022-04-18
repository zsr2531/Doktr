using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Fragments;

public enum TableAlignment
{
    Left,
    Center,
    Right
}

public class TableFragment : IDocumentationFragment
{
    public TableAlignment Alignment { get; set; } = TableAlignment.Left;
    public RowSegment Header { get; set; } = new();
    public RowSegmentCollection Rows { get; set; } = new();
    
    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitTable(this);
}