using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class ItemXmlDocSegment : IXmlDocSegment
    {
        public ItemXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
        {
            Content = content;
        }

        public string Name => "item";

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