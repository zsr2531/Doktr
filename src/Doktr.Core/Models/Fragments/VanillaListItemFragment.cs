using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class VanillaListItemFragment : ListItemFragment
{
    public DocumentationFragmentCollection Children { get; set; } = new();

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitVanillaListItem(this);
}