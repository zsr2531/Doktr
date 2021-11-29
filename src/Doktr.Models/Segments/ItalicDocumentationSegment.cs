using System.Collections.Immutable;

namespace Doktr.Models.Segments;

public class ItalicDocumentationSegment : IDocumentationSegment
{
    public ItalicDocumentationSegment(ImmutableArray<IDocumentationSegment> content)
    {
        Content = content;
    }

    public ImmutableArray<IDocumentationSegment> Content
    {
        get;
    }

    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
}