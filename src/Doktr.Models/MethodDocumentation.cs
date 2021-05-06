using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class MethodDocumentation : IMemberDocumentation
    {
        public MethodDocumentation(
            string name,
            ImmutableArray<IDocumentationSegment> summary,
            string syntax,
            ImmutableArray<TypeParameterDocumentation> typeParameters,
            ImmutableArray<ParameterDocumentation> parameters,
            IReference returnType,
            ImmutableArray<IDocumentationSegment> returns,
            ImmutableArray<IDocumentationSegment> examples,
            ImmutableArray<IDocumentationSegment> remarks)
        {
            Name = name;
            Summary = summary;
            Syntax = syntax;
            TypeParameters = typeParameters;
            Parameters = parameters;
            ReturnType = returnType;
            Returns = returns;
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
        
        public ImmutableArray<ParameterDocumentation> Parameters
        {
            get;
        }
        
        public IReference ReturnType
        {
            get;
        }
        
        public ImmutableArray<IDocumentationSegment> Returns
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