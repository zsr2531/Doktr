using System.Collections.ObjectModel;
using Doktr.Core.Models.Fragments;

namespace Doktr.Core.Models.Collections;

// TODO: Make a common base type for references so this can be nicer.
public class LinkDocumentationFragmentCollection : Collection<DocumentationFragment>, ICloneable
{
    protected override void InsertItem(int index, DocumentationFragment item)
    {
        AssertCorrectType(item);
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, DocumentationFragment item)
    {
        AssertCorrectType(item);
        base.SetItem(index, item);
    }

    public LinkDocumentationFragmentCollection Clone()
    {
        var clone = new LinkDocumentationFragmentCollection();
        foreach (var item in this)
            clone.Add(item.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();

    private static void AssertCorrectType(DocumentationFragment fragment)
    {
        if (fragment is not LinkReferenceFragment and not CodeReferenceFragment)
            throw new ArgumentException("Fragment must be of type LinkReferenceFragment or CodeReferenceFragment");
    }
}