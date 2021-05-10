using System.Collections.Immutable;

namespace Doktr.Models.Segments
{
    public class ParagraphDocumentationSegment : IDocumentationSegment
    {
        public ParagraphDocumentationSegment(ImmutableArray<IDocumentationSegment> content)
        {
            Content = content;
        }

        public ImmutableArray<IDocumentationSegment> Content
        {
            get;
        }

        public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
    }
}