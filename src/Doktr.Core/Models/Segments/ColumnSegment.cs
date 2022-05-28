using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class ColumnSegment : ICloneable
{
    public DocumentationFragmentCollection Content { get; set; } = new();

    public ColumnSegment Clone() => new()
    {
        Content = Content.Clone()
    };

    object ICloneable.Clone() => Clone();
}