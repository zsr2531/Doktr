namespace Doktr.Models.References
{
    public class UnresolvedReference : IReference
    {
        public UnresolvedReference(string cref)
        {
            Cref = cref;
        }

        public string Cref
        {
            get;
        }
        
        public string? Text
        {
            get;
            init;
        }
    }
}