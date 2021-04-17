using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class ListXmlDocSegment : IXmlDocSegment
    {
        public ListXmlDocSegment(string type, ImmutableArray<IXmlDocSegment> content)
        {
            Type = type;
            Content = content;
        }

        public string Name => "list";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Type
        {
            get;
        }
        
        public ImmutableArray<IXmlDocSegment> Content
        {
            get;
        }
    }
}