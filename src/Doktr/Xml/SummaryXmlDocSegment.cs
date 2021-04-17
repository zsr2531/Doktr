using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class SummaryXmlDocSegment : IXmlDocSegment
    {
        public SummaryXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "summary";

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