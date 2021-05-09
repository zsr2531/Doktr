using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class ExceptionDocumentation
    {
        public ExceptionDocumentation(IReference exception)
        {
            Exception = exception;
        }

        public IReference Exception
        {
            get;
        }
        
        public ImmutableArray<IDocumentationSegment> Documentation
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;
    }
}