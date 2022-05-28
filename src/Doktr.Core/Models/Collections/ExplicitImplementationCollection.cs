using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class ExplicitImplementationCollection : Collection<ExplicitImplementation>, ICloneable
{
    public ExplicitImplementationCollection Clone()
    {
        var clone = new ExplicitImplementationCollection();
        foreach (var item in this)
            clone.Add(item.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();
}