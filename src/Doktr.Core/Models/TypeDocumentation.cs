using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class TypeDocumentation : MemberDocumentation,
    IHasTypeParameters,
    IHasExtensionMethods
{
    protected TypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public TypeParameterSegmentCollection TypeParameters { get; set; } = new();
    public CodeReferenceCollection ExtensionMethods { get; set; } = new();

    public abstract override TypeDocumentation Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is not TypeDocumentation otherType)
            throw new ArgumentException("Cannot copy documentation to a non-type member.", nameof(other));

        otherType.TypeParameters = TypeParameters.Clone();
        otherType.ExtensionMethods = ExtensionMethods.Clone();
        base.CopyDocumentationTo(otherType);
    }
}