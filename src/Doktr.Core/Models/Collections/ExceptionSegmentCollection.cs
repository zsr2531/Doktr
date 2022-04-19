using System.Collections.ObjectModel;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Collections;

public class ExceptionSegmentCollection : Collection<ExceptionSegment>, ICloneable
{
    public ExceptionSegmentCollection Clone()
    {
        var clone = new ExceptionSegmentCollection();
        foreach (var item in this)
            clone.Add(item.Clone());
        
        return clone;
    }

    object ICloneable.Clone() => Clone();
}