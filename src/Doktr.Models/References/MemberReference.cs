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
    }
}