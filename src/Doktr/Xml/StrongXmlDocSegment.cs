using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class StrongXmlDocSegment : IXmlDocSegment
    {
        public StrongXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "strong";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ImmutableArray<IXmlDocSegment> Content
        {
            get;
        }
    }
}