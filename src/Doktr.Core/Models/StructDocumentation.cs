namespace Doktr.Core.Models;

public class StructDocumentation : CompositeTypeDocumentation, IHasReadOnly
{
    public StructDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public bool IsReadOnly { get; set; }
    public bool IsByRef { get; set; }

    public override StructDocumentation Clone()
    {
        var clone = new StructDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is StructDocumentation otherStruct)
        {
            otherStruct.IsReadOnly = IsReadOnly;
            otherStruct.IsByRef = IsByRef;
        }

        base.CopyDocumentationTo(other);
    }
}