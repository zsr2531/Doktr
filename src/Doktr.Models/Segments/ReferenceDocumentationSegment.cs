using Doktr.Models.References;

namespace Doktr.Models.Segments;

public class ReferenceDocumentationSegment : IDocumentationSegment
{
    public ReferenceDocumentationSegment(IReference reference)
    {
        Reference = reference;
    }

    public IReference Reference { get; }

    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);

    public override string ToString() => $"<cref = {{{Reference}}}>";
}