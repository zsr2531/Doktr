using System.Collections.Immutable;

namespace Doktr.Models.References
{
    public class MemberReference : IReference
    {
        public MemberReference(string cref, IMemberDocumentation target)
        {
            Cref = cref;
            Target = target;
        }

        public string Cref
        {
            get;
        }

        public IMemberDocumentation Target
        {
            get;
        }

        public ImmutableArray<IReference> GenericParameters
        {
            get;
            init;
        } = ImmutableArray<IReference>.Empty;
    }
}