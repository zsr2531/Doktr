using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Fragments;

public enum ListStyle
{
    Bullet,
    Numbered
}

public class ListFragment : IDocumentationFragment
{
    public ListStyle Style { get; set; } = ListStyle.Bullet;
    public ListItemFragmentCollection Items { get; set; } = new();

    public void AcceptVisitor(IDocumentationFragmentVisitor visitor) => visitor.VisitList(this);
}