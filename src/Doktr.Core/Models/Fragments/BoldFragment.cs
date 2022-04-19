using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class BoldFragment : DocumentationFragment
{
    public DocumentationFragmentCollection Children { get; set; } = new();

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitBold(this);

    public override BoldFragment Clone() => new()
    {
        Children = Children.Clone()
    };
}