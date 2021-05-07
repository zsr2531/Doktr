using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class TypeParameterDocumentation
    {
        public TypeParameterDocumentation(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
        
        public ImmutableArray<IDocumentationSegment> Documentation
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;
        
        public ImmutableArray<IReference> Constraints
        {
            get;
            init;
        } = ImmutableArray<IReference>.Empty;
    }
}