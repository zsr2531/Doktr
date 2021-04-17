using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class DescriptionXmlDocSegment : IXmlDocSegment
    {
        public DescriptionXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "description";

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