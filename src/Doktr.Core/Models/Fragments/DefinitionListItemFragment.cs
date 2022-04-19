using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public class DefinitionListItemFragment : ListItemFragment
{
    public DocumentationFragmentCollection Term { get; set; } = new();
    public DocumentationFragmentCollection Description { get; set; } = new();
    
    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitDefinitionListItem(this);

    public override DefinitionListItemFragment Clone() => new()
    {
        Term = Term.Clone(),
        Description = Description.Clone()
    };
}