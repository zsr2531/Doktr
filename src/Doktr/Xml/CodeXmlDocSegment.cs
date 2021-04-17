using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class CodeXmlDocSegment : IXmlDocSegment
    {
        public CodeXmlDocSegment(ImmutableArray<IXmlDocSegment> content)
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