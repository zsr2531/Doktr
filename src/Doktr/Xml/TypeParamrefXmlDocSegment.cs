using Doktr.Generation;

namespace Doktr.Xml
{
    public class TypeParamrefXmlDocSegment : IXmlDocSegment
    {
        public TypeParamrefXmlDocSegment(string typeParameter)
        {
            TypeParameter = typeParameter;
        }

        public string Name => "typepararmref";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string TypeParameter
        {
            get;
        }
    }
}