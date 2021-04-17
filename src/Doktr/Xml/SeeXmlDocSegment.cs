using Doktr.Generation;

namespace Doktr.Xml
{
    public class SeeXmlDocSegment : IXmlDocSegment
    {
        public SeeXmlDocSegment(string cref)
        {
            Cref = cref;
        }

        public string Name => "see";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Cref
        {
            get;
        }
    }
}