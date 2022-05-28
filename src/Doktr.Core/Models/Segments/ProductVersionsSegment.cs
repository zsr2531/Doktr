using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class ProductVersionsSegment : ICloneable
{
    public ProductVersionsSegment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public VersionCollection Versions { get; set; } = new();

    public ProductVersionsSegment Clone() => new(Name)
    {
        Versions = Versions.Clone()
    };

    object ICloneable.Clone() => Clone();
}