using Doktr.Generation;

namespace Doktr.Xml
{
    public interface IXmlDocSegment
    {
        string Name
        {
            get;
        }

        void AcceptVisitor(IDocumentationVisitor visitor);
    }
}