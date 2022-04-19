using System.Collections.ObjectModel;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Collections;

public class RowSegmentCollection : Collection<RowSegment>, ICloneable
{
    public RowSegmentCollection Clone()
    {
        var clone = new RowSegmentCollection();
        foreach (var segment in this)
            clone.Add(segment.Clone());
        
        return clone;
    }

    object ICloneable.Clone() => Clone();
}