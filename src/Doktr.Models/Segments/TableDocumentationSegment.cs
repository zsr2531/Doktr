using System.Collections.Immutable;

namespace Doktr.Models.Segments
{
    public class TableDocumentationSegment : IDocumentationSegment
    {
        public TableDocumentationSegment(
            ImmutableArray<TermDocumentationSegment> header,
            ImmutableArray<ImmutableArray<TermDocumentationSegment>> rows)
        {
            Header = header;
            Rows = rows;
        }

        public ImmutableArray<TermDocumentationSegment> Header
        {
            get;
        }
        
        public ImmutableArray<ImmutableArray<TermDocumentationSegment>> Rows
        {
            get;
        }
        
        public void AcceptVisitor(IDocumentationSegmentVisitor visitor) => visitor.Visit(this);
    }
}