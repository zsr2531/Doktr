using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class ColumnSegment
{
    public DocumentationFragmentCollection Content { get; set; } = new();
}