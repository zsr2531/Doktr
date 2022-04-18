using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class RowSegment
{
    public DocumentationFragmentCollection Columns { get; set; } = new();
}