using System.Collections.ObjectModel;
using Doktr.Core.Models.Segments;

namespace Doktr.Core.Models.Collections;

public class ProductVersionsSegmentCollection : Collection<ProductVersionsSegment>, ICloneable
{
    public ProductVersionsSegmentCollection Clone()
    {
        var clone = new ProductVersionsSegmentCollection();
        foreach (var segment in this)
            clone.Add(segment.Clone());

        return clone;
    }

    object ICloneable.Clone() => Clone();
}