using Doktr.Xml;

namespace Doktr.Generation
{
    public interface IDocumentationVisitor
    {
        void Visit(MonospaceXmlDocSegment segment);

        void Visit(DescriptionXmlDocSegment segment);

        void Visit(ExceptionXmlDocSegment segment);

        void Visit(InheritDocXmlDocSegment segment);

        void Visit(ItemXmlDocSegment segment);

        void Visit(ListXmlDocSegment segment);

        void Visit(ParamrefXmlDocSegment segment);

        void Visit(ParamXmlDocSegment segment);

        void Visit(ParaXmlDocSegment segment);

        void Visit(RawXmlDocSegment segment);

        void Visit(RemarksXmlDocSegment segment);

        void Visit(ReturnsXmlDocSegment segment);

        void Visit(SeealsoXmlDocSegment segment);

        void Visit(SeeXmlDocSegment segment);

        void Visit(StrongXmlDocSegment segment);

        void Visit(SummaryXmlDocSegment segment);
        
        void Visit(TypeParamrefXmlDocSegment segment);

        void Visit(TypeParamXmlDocSegment segment);
    }
}