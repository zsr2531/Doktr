namespace Doktr.Core.Models.Fragments;

public interface IDocumentationFragment
{
    void AcceptVisitor(IDocumentationFragmentVisitor visitor);
}