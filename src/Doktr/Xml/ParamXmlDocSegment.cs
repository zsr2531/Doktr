using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class ParamXmlDocSegment : IXmlDocSegment
    {
        public ParamXmlDocSegment(string parameter, ImmutableArray<IXmlDocSegment> content)
        {
            Parameter = parameter;
            Content = content;
        }

        public string Name => "param";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Parameter
        {
            get;
        }

        public ImmutableArray<IXmlDocSegment> Content
        {
            get;
        }
    }
}