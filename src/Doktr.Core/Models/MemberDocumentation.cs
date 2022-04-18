using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class MemberDocumentation
{
    protected MemberDocumentation(string name, MemberVisibility visibility)
    {
        Name = name;
        Visibility = visibility;
    }

    public string Name { get; }
    public MemberVisibility Visibility { get; }
    public DocumentationFragmentCollection Summary { get; set; } = new();
    public DocumentationFragmentCollection Examples { get; set; } = new();
    public DocumentationFragmentCollection Remarks { get; set; } = new();
    public ProductVersionsSegmentCollection AppliesTo { get; set; } = new();
}