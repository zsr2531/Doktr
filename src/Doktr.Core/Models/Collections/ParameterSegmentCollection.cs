using System.Collections.ObjectModel;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Collections;

public class ParameterSegmentCollection : Collection<ParameterSegment>, ICloneable
{
    public ParameterSegmentCollection Clone()
    {
        var clone = new ParameterSegmentCollection();
        foreach (var segment in this)
            clone.Add(segment.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();
}