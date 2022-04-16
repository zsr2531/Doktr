namespace Doktr.Core.Models.Fragments;

public abstract class DocumentationFragment
{
    public abstract void AcceptVisitor<T>(IDocumentationFragmentVisitor<T> visitor);
}