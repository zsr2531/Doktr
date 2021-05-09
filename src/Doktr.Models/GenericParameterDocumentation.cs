using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class GenericParameterDocumentation
    {
        public GenericParameterDocumentation(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }

        public GenericParameterModifier Modifier
        {
            get;
            init;
        } = GenericParameterModifier.None;
        
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