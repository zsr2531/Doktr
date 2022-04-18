namespace Doktr.Core.Models.Fragments;

public class ParameterReferenceFragment : IDocumentationFragment
{
    public ParameterReferenceFragment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitParameterReference(this);
}