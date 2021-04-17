using Doktr.Generation;

namespace Doktr.Xml
{
    public class RawXmlDocSegment : IXmlDocSegment
    {
        public RawXmlDocSegment(string content)
        {
            Content = content;
        }

        public string Name => string.Empty;

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Content
        {
            get;
        }
    }
}