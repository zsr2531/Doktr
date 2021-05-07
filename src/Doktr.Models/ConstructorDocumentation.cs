using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class ConstructorDocumentation : IMemberDocumentation
    {
        public ConstructorDocumentation(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
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

        public ImmutableArray<ParameterDocumentation> Parameters
        {
            get;
            init;
        } = ImmutableArray<ParameterDocumentation>.Empty;

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