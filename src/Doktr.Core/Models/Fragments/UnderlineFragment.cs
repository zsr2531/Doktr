using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class UnderlineFragment : DocumentationFragment
{
    public DocumentationFragmentCollection Children { get; set; } = new();

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitUnderline(this);

    public override UnderlineFragment Clone() => new()
    {
        Children = Children.Clone()
    };
}