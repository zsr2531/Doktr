using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class RemarksXmlDocSegment : IXmlDocSegment
    {
        public RemarksXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "remarks";

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