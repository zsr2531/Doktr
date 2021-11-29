namespace Doktr.Models.Segments;

public class MonospaceDocumentationSegment : IDocumentationSegment
{
    public MonospaceDocumentationSegment(string content)
    {
        Content = content;
    }

    public string Content
    {
        get;
    }
        
    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
}