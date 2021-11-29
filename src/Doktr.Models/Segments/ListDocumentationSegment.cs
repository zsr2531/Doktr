using System.Collections.Immutable;

namespace Doktr.Models.Segments;

public class ListDocumentationSegment : IDocumentationSegment
{
    public ListDocumentationSegment(ListType type, ImmutableArray<ImmutableArray<IDocumentationSegment>> items)
    {
        Type = type;
        Items = items;
    }

    public ListType Type
    {
        get;
    }
        
    public ImmutableArray<ImmutableArray<IDocumentationSegment>> Items
    {
        get;
    }

    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
}