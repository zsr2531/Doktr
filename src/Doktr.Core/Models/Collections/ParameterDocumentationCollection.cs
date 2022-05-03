using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class ParameterDocumentationCollection : Collection<ParameterDocumentation>, ICloneable
{
    public ParameterDocumentationCollection Clone()
    {
        var clone = new ParameterDocumentationCollection();
        foreach (var segment in this)
            clone.Add(segment.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();
}