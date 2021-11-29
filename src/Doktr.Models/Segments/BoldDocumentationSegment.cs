using System.Collections.Immutable;

namespace Doktr.Models.Segments;

public class BoldDocumentationSegment : IDocumentationSegment
{
    public BoldDocumentationSegment(ImmutableArray<IDocumentationSegment> content)
    {
        Content = content;
    }

    public ImmutableArray<IDocumentationSegment> Content
    {
        get;
    }

    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
}