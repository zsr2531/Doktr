namespace Doktr.Models.Segments
{
    public class CodeBlockDocumentationSegment : IDocumentationSegment
    {
        public CodeBlockDocumentationSegment(string content)
        {
            Content = content;
        }

        public string Content
        {
            get;
        }

        public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
    }
}