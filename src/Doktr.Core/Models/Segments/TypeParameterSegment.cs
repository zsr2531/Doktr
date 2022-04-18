using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

// TODO: Constraints!
public class TypeParameterSegment
{
    public TypeParameterSegment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public DocumentationFragmentCollection Documentation { get; set; } = new();
}