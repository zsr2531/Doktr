using System.Collections.ObjectModel;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Collections;

public class ColumnSegmentCollection : Collection<ColumnSegment>, ICloneable
{
    public ColumnSegmentCollection Clone()
    {
        var clone = new ColumnSegmentCollection();
        foreach (var segment in this)
            clone.Add(segment.Clone());
        
        return clone;
    }
    
    object ICloneable.Clone() => Clone();
}