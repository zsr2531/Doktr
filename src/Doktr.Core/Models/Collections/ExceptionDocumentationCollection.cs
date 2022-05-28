using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class ExceptionDocumentationCollection : Collection<ExceptionDocumentation>, ICloneable
{
    public ExceptionDocumentationCollection Clone()
    {
        var clone = new ExceptionDocumentationCollection();
        foreach (var item in this)
            clone.Add(item.Clone());
        
        return clone;
    }

    object ICloneable.Clone() => Clone();
}