namespace Doktr.Core.Models.Fragments;

public class ParameterReferenceFragment : DocumentationFragment
{
    public ParameterReferenceFragment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitParameterReference(this);

    public override ParameterReferenceFragment Clone() => new(Name);
}