namespace Doktr.Models.Segments;

public interface IDocumentationSegmentVisitor
{
    void Visit(TextDocumentationSegment segment);

    void Visit(BoldDocumentationSegment segment);

    void Visit(ItalicDocumentationSegment segment);

    void Visit(ReferenceDocumentationSegment segment);

    void Visit(MonospaceDocumentationSegment segment);

    void Visit(CodeBlockDocumentationSegment segment);

    void Visit(ParagraphDocumentationSegment segment);

    void Visit(ListDocumentationSegment segment);

    void Visit(TableDocumentationSegment segment);

    void Visit(LinkDocumentationSegment segment);
}