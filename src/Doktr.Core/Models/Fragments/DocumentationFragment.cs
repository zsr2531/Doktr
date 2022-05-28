namespace Doktr.Core.Models.Fragments;

public abstract class DocumentationFragment : ICloneable
{
    public abstract void AcceptVisitor(IDocumentationFragmentVisitor visitor);

    public abstract DocumentationFragment Clone();

    object ICloneable.Clone() => Clone();
}