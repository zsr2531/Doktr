namespace Doktr.Models.Segments;

public class TextDocumentationSegment : IDocumentationSegment
{
    public TextDocumentationSegment(string content)
    {
        Content = content;
    }

    public string Content
    {
        get;
    }

    public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
}