using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class ReturnsXmlDocSegment : IXmlDocSegment
    {
        public ReturnsXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "returns";

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