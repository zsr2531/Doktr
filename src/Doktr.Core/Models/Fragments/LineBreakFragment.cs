namespace Doktr.Core.Models.Fragments;

public class LineBreakFragment : IDocumentationFragment
{
    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitLineBreak(this);
}