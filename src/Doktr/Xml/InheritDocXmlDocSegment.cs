using Doktr.Generation;

namespace Doktr.Xml
{
    public class InheritDocXmlDocSegment : IXmlDocSegment
    {
        public InheritDocXmlDocSegment(string from = null)
        {
            From = from;
        }

        public string Name => "inheritdoc";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string From
        {
            get;
        }
    }
}