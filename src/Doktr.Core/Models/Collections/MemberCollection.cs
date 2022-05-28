using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class MemberCollection<T> : Collection<T>, ICloneable
    where T : MemberDocumentation
{
    public MemberCollection<T> Clone()
    {
        var clone = new MemberCollection<T>();
        foreach (var item in this)
            clone.Add((T) item.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();
}