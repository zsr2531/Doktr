using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class TypeDocumentation : MemberDocumentation, IHasStatic, IHasTypeParameters, IHasExtensionMethods
{
    protected TypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public bool IsStatic { get; set; }
    public TypeParameterSegmentCollection TypeParameters { get; set; } = new();
    public ExtensionMethodCollection ExtensionMethods { get; set; } = new();

    public abstract override TypeDocumentation Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is not TypeDocumentation otherType)
            throw new InvalidOperationException("Cannot copy documentation to a non-type member.");

        otherType.IsStatic = IsStatic;
        otherType.TypeParameters = TypeParameters.Clone();
        otherType.ExtensionMethods = ExtensionMethods.Clone();
        base.CopyDocumentationTo(otherType);
    }
}