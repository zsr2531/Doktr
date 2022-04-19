using System.Collections.ObjectModel;
using Doktr.Core.Models.Fragments;

namespace Doktr.Core.Models.Collections;

public class DocumentationFragmentCollection : Collection<DocumentationFragment>, ICloneable
{
    public void AcceptVisitor(IDocumentationFragmentVisitor visitor)
    {
        foreach (var fragment in this)
            fragment.AcceptVisitor(visitor);
    }


    public DocumentationFragmentCollection Clone()
    {
        var clone = new DocumentationFragmentCollection();
        foreach (var fragment in this)
            clone.Add(fragment.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();
}