using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class TypeParameterDocumentationCollection : Collection<TypeParameterDocumentation>, ICloneable
{
    public TypeParameterDocumentationCollection Clone()
    {
        var clone = new TypeParameterDocumentationCollection();
        foreach (var segment in this)
            clone.Add(segment.Clone());
        
        return clone;
    }
    
    object ICloneable.Clone() => Clone();
}