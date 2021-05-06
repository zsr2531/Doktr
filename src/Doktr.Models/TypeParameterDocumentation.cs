using System.Collections.Immutable;
using Doktr.Models.Segments;

using Constraint = LanguageExt.Either<Doktr.Models.References.IReference, string>;

namespace Doktr.Models
{
    public class TypeParameterDocumentation
    {
        public TypeParameterDocumentation(
            string name,
            ImmutableArray<IDocumentationSegment> documentation,
            ImmutableArray<Constraint> constraints)
        {
            Name = name;
            Documentation = documentation;
            Constraints = constraints;
        }

        public string Name
        {
            get;
        }
        
        public ImmutableArray<IDocumentationSegment> Documentation
        {
            get;
        }
        
        public ImmutableArray<Constraint> Constraints
        {
            get;
        }
    }
}