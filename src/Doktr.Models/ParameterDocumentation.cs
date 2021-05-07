using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class ParameterDocumentation
    {
        public ParameterDocumentation(string name, IReference type)
        {
            Name = name;
            Type = type;
        }

        public IReference Type
        {
            get;
            init;
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
    }
}