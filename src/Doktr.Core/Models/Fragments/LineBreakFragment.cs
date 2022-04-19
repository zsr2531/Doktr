namespace Doktr.Core.Models.Fragments;

public class LineBreakFragment : DocumentationFragment
{
    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitLineBreak(this);

    public override LineBreakFragment Clone() => new();
}