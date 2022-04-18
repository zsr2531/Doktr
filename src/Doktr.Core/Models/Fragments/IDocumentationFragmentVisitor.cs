namespace Doktr.Core.Models.Fragments;

public interface IDocumentationFragmentVisitor
{
    void VisitText(TextSegment textSegment);

    void VisitBold(BoldSegment boldSegment);

    void VisitItalic(ItalicSegment italicSegment);

    void VisitMonospace(MonospaceSegment monospaceSegment);

    void VisitCode(CodeSegment codeSegment);

    void VisitParagraph(ParagraphFragment paragraphFragment);

    void VisitParameterReference(ParameterReferenceFragment parameterReferenceFragment);

    void VisitTypeParameterReference(TypeParameterReferenceFragment typeParameterReferenceFragment);
}