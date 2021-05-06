using System.Collections.Immutable;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public abstract class TypeDocumentation : IMemberDocumentation
    {
        protected TypeDocumentation(
            string name,
            ImmutableArray<IDocumentationSegment> summary,
            string syntax,
            ImmutableArray<TypeParameterDocumentation> typeParameters,
            ImmutableArray<IDocumentationSegment> examples,
            ImmutableArray<IDocumentationSegment> remarks)
        {
            Name = name;
            Summary = summary;
            Syntax = syntax;
            TypeParameters = typeParameters;
            Examples = examples;
            Remarks = remarks;
        }

        public string Name
        {
            get;
        }

        public ImmutableArray<IDocumentationSegment> Summary
        {
            get;
        }

        public string Syntax
        {
            get;
        }
        
        public ImmutableArray<TypeParameterDocumentation> TypeParameters
        {
            get;
        }

        public ImmutableArray<IDocumentationSegment> Examples
        {
            get;
        }

        public ImmutableArray<IDocumentationSegment> Remarks
        {
            get;
        }
    }
}