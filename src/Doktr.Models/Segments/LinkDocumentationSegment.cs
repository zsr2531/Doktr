namespace Doktr.Models.Segments;

public class LinkDocumentationSegment : IDocumentationSegment
{
    public LinkDocumentationSegment(string url)
    {
        Url = url;
    }

    public string Url { get; }

    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
}