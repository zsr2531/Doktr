using System.Collections.ObjectModel;
using Doktr.Core.Models.Fragments;

namespace Doktr.Core.Models.Collections;

public class ListItemFragmentCollection : Collection<ListItemFragment>
{
    public void AcceptVisitor(IDocumentationFragmentVisitor visitor)
    {
        foreach (var fragment in this)
            fragment.AcceptVisitor(visitor);
    }
}