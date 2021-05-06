namespace Doktr.Models.References
{
    public class ResolvedReference : IReference
    {
        public ResolvedReference(string cref, IMemberDocumentation target)
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