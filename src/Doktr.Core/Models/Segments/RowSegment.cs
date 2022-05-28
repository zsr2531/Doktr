using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class RowSegment : ICloneable
{
    public ColumnSegmentCollection Columns { get; set; } = new();

    public RowSegment Clone() => new()
    {
        Columns = Columns.Clone()
    };

    object ICloneable.Clone() => Clone();
}