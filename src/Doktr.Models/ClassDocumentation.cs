using System.Collections.Immutable;
using Doktr.Models.References;

namespace Doktr.Models
{
    public class ClassDocumentation : TypeDocumentation
    {
        public ClassDocumentation(string assembly, string ns, string name, IReference baseType)
            : base(assembly, ns, name)
        {
            BaseType = baseType;
        }
        
        public IReference BaseType
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