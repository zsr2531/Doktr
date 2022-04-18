using System.Collections.ObjectModel;
using Doktr.Core.Models.Fragments;

namespace Doktr.Core.Models.Collections;

public class DocumentationFragmentCollection : Collection<IDocumentationFragment>
{
    public void AcceptVisitor(IDocumentationFragmentVisitor visitor)
    {
        foreach (var fragment in this)
            fragment.AcceptVisitor(visitor);
    }
}