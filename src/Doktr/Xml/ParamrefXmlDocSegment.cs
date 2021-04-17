using Doktr.Generation;

namespace Doktr.Xml
{
    public class ParamrefXmlDocSegment : IXmlDocSegment
    {
        public ParamrefXmlDocSegment(string parameter)
        {
            Parameter = parameter;
        }

        public string Name => "paramref";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Parameter
        {
            get;
        }
    }
}