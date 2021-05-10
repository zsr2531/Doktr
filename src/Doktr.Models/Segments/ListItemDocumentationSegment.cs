using System.Collections.Immutable;

namespace Doktr.Models.Segments
{
    public class ListItemDocumentationSegment
    {
        public ListItemDocumentationSegment(
            ImmutableArray<IDocumentationSegment> term,
            ImmutableArray<IDocumentationSegment> content)
        {
            Term = term;
            Content = content;
        }

        public ImmutableArray<IDocumentationSegment> Term
        {
            get;
        }
        
        public ImmutableArray<IDocumentationSegment> Content
        {
            get;
        }
    }
}