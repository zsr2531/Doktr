using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class FieldDocumentation : IMemberDocumentation
    {
        public FieldDocumentation(
            string name,
            ImmutableArray<IDocumentationSegment> summary,
            string syntax,
            IReference type,
            ImmutableArray<IDocumentationSegment> examples,
            ImmutableArray<IDocumentationSegment> remarks)
        {
            Name = name;
            Summary = summary;
            Syntax = syntax;
            Type = type;
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
        
        public IReference Type
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