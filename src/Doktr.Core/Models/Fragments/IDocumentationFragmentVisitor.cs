namespace Doktr.Core.Models.Fragments;

public interface IDocumentationFragmentVisitor
{
    void VisitText(TextFragment textFragment);

    void VisitBold(BoldFragment boldFragment);

    void VisitItalic(ItalicFragment italicFragment);

    void VisitMonospace(MonospaceFragment monospaceFragment);

    void VisitCode(CodeFragment codeFragment);

    void VisitParagraph(ParagraphFragment paragraphFragment);

    void VisitParameterReference(ParameterReferenceFragment parameterReferenceFragment);

    void VisitTypeParameterReference(TypeParameterReferenceFragment typeParameterReferenceFragment);

    void VisitLinkReference(LinkReferenceFragment linkReferenceFragment);

    void VisitCodeReference(CodeReferenceFragment codeReferenceFragment);
}