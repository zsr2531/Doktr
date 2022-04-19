using System.Collections.ObjectModel;
using Doktr.Core.Models.Fragments;

namespace Doktr.Core.Models.Collections;

public class ListItemFragmentCollection : Collection<ListItemFragment>, ICloneable
{
    public void AcceptVisitor(IDocumentationFragmentVisitor visitor)
    {
        foreach (var fragment in this)
            fragment.AcceptVisitor(visitor);
    }

    public ListItemFragmentCollection Clone()
    {
        var clone = new ListItemFragmentCollection();
        foreach (var fragment in this)
            clone.Add(fragment.Clone());
        
        return clone;
    }

    object ICloneable.Clone() => Clone();
}