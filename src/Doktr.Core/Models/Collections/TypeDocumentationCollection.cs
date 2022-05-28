using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class TypeDocumentationCollection : Collection<TypeDocumentation>, ICloneable
{
    public TypeDocumentationCollection Clone()
    {
        var clone = new TypeDocumentationCollection();
        foreach (var type in this)
            clone.Add(type.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();
}