using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class EventDocumentation : IMemberDocumentation
    {
        public EventDocumentation(string name, IReference type)
        {
            Name = name;
            Type = type;
        }

        public string Name
        {
            get;
        }

        public IReference Type
        {
            get;
            init;
        }

        public ImmutableArray<IDocumentationSegment> Summary
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;

        public string? Syntax
        {
            get;
            init;
        }

        public ImmutableArray<IDocumentationSegment> Examples
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;

        public ImmutableArray<IDocumentationSegment> Remarks
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;

        public ImmutableArray<IReference> SeeAlso
        {
            get;
            init;
        } = ImmutableArray<IReference>.Empty;
    }
}