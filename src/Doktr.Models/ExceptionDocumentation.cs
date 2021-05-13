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

        public ExceptionDocumentation(ExceptionDocumentation other)
        {
            Exception = other.Exception;
            Documentation = other.Documentation;
        }

        public IReference Exception
        {
            get;
            init;
        }
        
        public ImmutableArray<IDocumentationSegment> Documentation
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;
    }
}