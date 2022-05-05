namespace Doktr.Core.Models.Fragments;

public interface IDocumentationFragmentVisitor
{
    void VisitText(TextFragment fragment);

    void VisitBold(BoldFragment fragment);

    void VisitItalic(ItalicFragment fragment);

    void VisitUnderline(UnderlineFragment fragment);

    void VisitMonospace(MonospaceFragment fragment);

    void VisitCode(CodeFragment fragment);

    void VisitParagraph(ParagraphFragment fragment);

    void VisitParameterReference(ParameterReferenceFragment fragment);

    void VisitTypeParameterReference(TypeParameterReferenceFragment fragment);

    void VisitLinkReference(LinkReferenceFragment fragment);

    void VisitCodeReference(CodeReferenceFragment fragment);

    void VisitLineBreak(LineBreakFragment fragment);

    void VisitList(ListFragment fragment);

    void VisitVanillaListItem(VanillaListItemFragment fragment);

    void VisitDefinitionListItem(DefinitionListItemFragment fragment);

    void VisitTable(TableFragment fragment);
}