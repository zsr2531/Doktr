using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class ExceptionXmlDocSegment : IXmlDocSegment
    {
        public ExceptionXmlDocSegment(string cref, ImmutableArray<IXmlDocSegment> content)
        {
            Cref = cref;
            Content = content;
        }

        public string Name => "exception";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Cref
        {
            get;
        }
        
        public ImmutableArray<IXmlDocSegment> Content
        {
            get;
        }
    }
}