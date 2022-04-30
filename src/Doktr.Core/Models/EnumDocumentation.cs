using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class EnumDocumentation : TypeDocumentation, IHasBaseType
{
    public EnumDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public TypeSignature? BaseType { get; set; }
    public bool IsFlags { get; set; }
    public MemberCollection<FieldDocumentation> Fields { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitEnum(this);

    public override EnumDocumentation Clone()
    {
        var clone = new EnumDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is EnumDocumentation otherEnum)
        {
            otherEnum.BaseType = BaseType?.Clone();
            otherEnum.IsFlags = IsFlags;
            otherEnum.Fields = Fields.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}