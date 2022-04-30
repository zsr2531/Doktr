using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public abstract class TypeDocumentation : MemberDocumentation,
    IHasExtensionMethods
{
    protected TypeDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public CodeReferenceCollection ExtensionMethods { get; set; } = new();

    public abstract override void AcceptVisitor(IDocumentationMemberVisitor visitor);

    public abstract override TypeDocumentation Clone();

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is TypeDocumentation otherType)
        {
            otherType.ExtensionMethods = ExtensionMethods.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}