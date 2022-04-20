using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class TypeDocumentation : MemberDocumentation,
    IHasStatic,
    IHasAbstract,
    IHasTypeParameters,
    IHasExtensionMethods
{
    protected TypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public bool IsStatic { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public TypeParameterSegmentCollection TypeParameters { get; set; } = new();
    public CodeReferenceCollection ExtensionMethods { get; set; } = new();

    public abstract override TypeDocumentation Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is not TypeDocumentation otherType)
            throw new ArgumentException("Cannot copy documentation to a non-type member.", nameof(other));

        otherType.IsStatic = IsStatic;
        otherType.IsAbstract = IsAbstract;
        otherType.IsSealed = IsSealed;
        otherType.TypeParameters = TypeParameters.Clone();
        otherType.ExtensionMethods = ExtensionMethods.Clone();
        base.CopyDocumentationTo(otherType);
    }
}