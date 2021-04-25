using Doktr.Generation;

namespace Doktr.Xml.Semantics
{
    public interface IDocumentationElement
    {
        void Visit(IDocumentationVisitor visitor);
    }
}