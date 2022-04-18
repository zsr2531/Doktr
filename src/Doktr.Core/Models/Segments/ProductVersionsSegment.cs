using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class ProductVersionsSegment
{
    public ProductVersionsSegment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public VersionCollection Versions { get; set; } = new();
}