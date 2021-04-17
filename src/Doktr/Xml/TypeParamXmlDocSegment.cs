using System.Collections.Immutable;
using Doktr.Generation;

namespace Doktr.Xml
{
    public class TypeParamXmlDocSegment : IXmlDocSegment
    {
        public TypeParamXmlDocSegment(string typeParameter, ImmutableArray<IXmlDocSegment> content)
        {
            TypeParameter = typeParameter;
            Content = content;
        }

        public string Name => "typeparam";

        public void AcceptVisitor(IDocumentationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string TypeParameter
        {
            get;
        }
        
        public ImmutableArray<IXmlDocSegment> Content
        {
            get;
        }
    }
}