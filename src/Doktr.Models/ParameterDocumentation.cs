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

        public string Name
        {
            get;
        }

        public IReference Type
        {
            get;
        }

        public ParameterModifier Modifier
        {
            get;
            init;
        } = ParameterModifier.None;

        public ImmutableArray<IDocumentationSegment> Documentation
        {
            get;
            init;
        } = ImmutableArray<IDocumentationSegment>.Empty;
    }
}