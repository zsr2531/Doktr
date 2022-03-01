using System.Collections.Immutable;

namespace Doktr.Models.Segments;

public class TableDocumentationSegment : IDocumentationSegment
{
    public TableDocumentationSegment(
        ImmutableArray<ImmutableArray<IDocumentationSegment>> header,
        ImmutableArray<ImmutableArray<ImmutableArray<IDocumentationSegment>>> rows)
    {
        Header = header;
        Rows = rows;
    }

    public ImmutableArray<ImmutableArray<IDocumentationSegment>> Header { get; }

    public ImmutableArray<ImmutableArray<ImmutableArray<IDocumentationSegment>>> Rows { get; }

    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
}