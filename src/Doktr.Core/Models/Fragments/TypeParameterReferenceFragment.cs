namespace Doktr.Core.Models.Fragments;

public class TypeParameterReferenceFragment : IDocumentationFragment
{
    public TypeParameterReferenceFragment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitTypeParameterReference(this);
}