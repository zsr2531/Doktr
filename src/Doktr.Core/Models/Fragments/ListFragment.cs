using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public enum ListStyle
{
    Bullet,
    Numbered
}

public class ListFragment : DocumentationFragment
{
    public ListStyle Style { get; set; } = ListStyle.Bullet;
    public ListItemFragmentCollection Items { get; set; } = new();

    public override void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitList(this);

    public override ListFragment Clone() => new()
    {
        Style = Style,
        Items = Items.Clone()
    };
}