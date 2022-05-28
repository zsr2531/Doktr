using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class MonospaceFragment : DocumentationFragment
{
    public DocumentationFragmentCollection Children { get; set; } = new();

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitMonospace(this);

    public override MonospaceFragment Clone() => new()
    {
        Children = Children.Clone()
    };
}