using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class ParaXmlDocSegment : IXmlDocSegment
    {
        public ParaXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "para";

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