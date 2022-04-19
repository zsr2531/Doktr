namespace Doktr.Core.Models.Fragments;

public class TypeParameterReferenceFragment : DocumentationFragment
{
    public TypeParameterReferenceFragment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitTypeParameterReference(this);

    public override TypeParameterReferenceFragment Clone() => new(Name);
}