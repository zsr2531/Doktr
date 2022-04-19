using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class ItalicFragment : DocumentationFragment
{
    public DocumentationFragmentCollection Children { get; set; } = new();

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitItalic(this);

    public override ItalicFragment Clone() => new()
    {
        Children = Children.Clone()
    };
}