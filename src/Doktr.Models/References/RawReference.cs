namespace Doktr.Models.References
{
    public class RawReference : IReference
    {
        public RawReference(string cref, string text)
        {
            Cref = cref;
            Text = text;
        }

        public string Cref
        {
            get;
        }

        public string Text
        {
            get;
        }
    }
}