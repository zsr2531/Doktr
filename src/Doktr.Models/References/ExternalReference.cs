namespace Doktr.Models.References
{
    public class ExternalReference : IReference
    {
        public ExternalReference(string cref, string url)
        {
            Cref = cref;
            Url = url;
        }

        public string Cref
        {
            get;
        }
        
        public string Url
        {
            get;
        }
    }
}