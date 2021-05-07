using System.Collections.Immutable;
using Doktr.Models.References;

namespace Doktr.Models
{
    public class RecordDocumentation : TypeDocumentation
    {
        public RecordDocumentation(
            string assembly,
            string ns,
            string name,
            IReference baseType,
            ImmutableArray<ParameterDocumentation> elements)
            : base(assembly, ns, name)
        {
            Elements = elements;
            BaseType = baseType;
        }
        
        public IReference BaseType
        {
            get;
        }
        
        public ImmutableArray<ParameterDocumentation> Elements
        {
            get;
        }
        
        public ImmutableArray<IReference> Implementations
        {
            get;
            init;
        } = ImmutableArray<IReference>.Empty;
    }
}