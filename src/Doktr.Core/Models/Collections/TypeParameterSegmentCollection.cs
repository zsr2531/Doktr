using System.Collections.ObjectModel;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Collections;

public class TypeParameterSegmentCollection : Collection<TypeParameterSegment>, ICloneable
{
    public TypeParameterSegmentCollection Clone()
    {
        var clone = new TypeParameterSegmentCollection();
        foreach (var segment in this)
            clone.Add(segment.Clone());
        
        return clone;
    }
    
    object ICloneable.Clone() => Clone();
}