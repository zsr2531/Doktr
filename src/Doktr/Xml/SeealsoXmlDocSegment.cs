using Doktr.Generation;

namespace Doktr.Xml
{
    public class SeealsoXmlDocSegment : IXmlDocSegment
    {
        public SeealsoXmlDocSegment(string cref)
        {
            Cref = cref;
        }

        public string Name => "seealso";

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
