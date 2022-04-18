namespace Doktr.Core.Models.Fragments;

public abstract class ListItemFragment : IDocumentationFragment
{
    public abstract void AcceptVisitor(IDocumentationFragmentVisitor visitor);
}