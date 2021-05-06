using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models
{
    public class ParameterDocumentation
    {
        public ParameterDocumentation(IReference type, string name, ImmutableArray<IDocumentationSegment> documentation)
        {
            Type = type;
            Name = name;
            Documentation = documentation;
        }

        public IReference Type
        {
            get;
        }
        
        public string Name
        {
            get;
        }
        
        public ImmutableArray<IDocumentationSegment> Documentation
        {
            get;
        }
    }
}