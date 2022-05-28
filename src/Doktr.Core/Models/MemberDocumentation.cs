using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class MemberDocumentation : ICloneable
{
    protected MemberDocumentation(string name, MemberVisibility visibility)
    {
        Name = name;
        Visibility = visibility;
    }

    public string Name { get; set; }
    public MemberVisibility Visibility { get; set; }
    public DocumentationFragmentCollection Summary { get; set; } = new();
    public DocumentationFragmentCollection Examples { get; set; } = new();
    public DocumentationFragmentCollection Remarks { get; set; } = new();
    public LinkDocumentationFragmentCollection SeeAlso { get; set; } = new();
    public ProductVersionsSegmentCollection AppliesTo { get; set; } = new();

    public abstract void AcceptVisitor(IDocumentationMemberVisitor visitor);

    public abstract MemberDocumentation Clone();

    object ICloneable.Clone() => Clone();
    
    protected virtual void CopyDocumentationTo(MemberDocumentation other)
    {
        other.Summary = Summary.Clone();
        other.Examples = Examples.Clone();
        other.Remarks = Remarks.Clone();
        other.SeeAlso = SeeAlso.Clone();
        other.AppliesTo = AppliesTo.Clone();
    }
}