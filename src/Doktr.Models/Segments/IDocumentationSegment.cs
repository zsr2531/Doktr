namespace Doktr.Models.Segments;

public interface IDocumentationSegment
{
    void AcceptVisitor(IDocumentationSegmentVisitor visitor);
}