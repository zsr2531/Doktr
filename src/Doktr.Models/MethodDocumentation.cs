using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class MethodDocumentation : IMemberDocumentation
    {
        public MethodDocumentation(string name, IReference returnType)
        {
            Name = name;
            ReturnType = returnType;
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

        public ImmutableArray<TypeParameterDocumentation> TypeParameters
        {
            get;
            init;
        } = ImmutableArray<TypeParameterDocumentation>.Empty;

        public ImmutableArray<ParameterDocumentation> Parameters
        {
            get;
            init;
        } = ImmutableArray<ParameterDocumentation>.Empty;

        public IReference ReturnType
        {
            get;
            init;
        }

        public ImmutableArray<IDocumentationSegment> Returns
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;

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