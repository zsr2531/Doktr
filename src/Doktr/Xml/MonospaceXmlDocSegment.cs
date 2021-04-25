using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class MonospaceXmlDocSegment : IXmlDocSegment
    {
        public MonospaceXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "c";

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