using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

// TODO: Modifiers! (in, out, ref)
public class ParameterSegment
{
    public ParameterSegment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public DocumentationFragmentCollection Documentation { get; set; } = new();
}