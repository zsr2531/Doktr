using System.Collections.Immutable;
using Doktr.Models.References;

namespace Doktr.Models
{
    public class InterfaceDocumentation : TypeDocumentation
    {
        public InterfaceDocumentation(string assembly, string ns, string name)
            : base(assembly, ns, name)
        {
        }
        
        public ImmutableArray<IReference> Implementations
        {
            get;
            init;
        } = ImmutableArray<IReference>.Empty;
    }
}