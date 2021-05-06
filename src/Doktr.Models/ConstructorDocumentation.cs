using System.Collections.Immutable;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class ConstructorDocumentation : IMemberDocumentation
    {
        public ConstructorDocumentation(
            string name,
            ImmutableArray<IDocumentationSegment> summary,
            string syntax,
            ImmutableArray<ParameterDocumentation> parameters,
            ImmutableArray<IDocumentationSegment> examples,
            ImmutableArray<IDocumentationSegment> remarks)
        {
            Name = name;
            Summary = summary;
            Syntax = syntax;
            Parameters = parameters;
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

        public ImmutableArray<ParameterDocumentation> Parameters
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