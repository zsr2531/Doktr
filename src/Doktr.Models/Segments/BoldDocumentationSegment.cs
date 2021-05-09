using System.Collections.Immutable;

namespace Doktr.Models.Segments
{
    public class BoldDocumentationSegment : IDocumentationSegment
    {
        public ImmutableArray<IDocumentationSegment> Content
        {
            get;
        }

        public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
    }
}