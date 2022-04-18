using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class RowSegment
{
    public ColumnSegmentCollection Columns { get; set; } = new();
}