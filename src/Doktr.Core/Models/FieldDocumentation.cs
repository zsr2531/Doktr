using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Constants;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class FieldDocumentation : MemberDocumentation, IHasStatic, IHasReadOnly, IHasValue
{
    public FieldDocumentation(string name, MemberVisibility visibility, TypeSignature type)
        : base(name, visibility)
    {
        Type = type;
    }
    
    public TypeSignature Type { get; set; }
    public bool IsStatic { get; set; }
    public bool IsVolatile { get; set; }
    [MemberNotNullWhen(true, nameof(ConstantValue))]
    public bool IsConstant { get; set; }
    public bool IsReadOnly { get; set; }
    public Constant? ConstantValue { get; set; }
    public DocumentationFragmentCollection Value { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitField(this);

    public override FieldDocumentation Clone()
    {
        var clone = new FieldDocumentation(Name, Visibility, Type.Clone());
        CopyDocumentationTo(clone);
        
        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is FieldDocumentation otherField)
        {
            otherField.IsStatic = IsStatic;
            otherField.IsVolatile = IsVolatile;
            otherField.IsConstant = IsConstant;
            otherField.IsReadOnly = IsReadOnly;
            otherField.ConstantValue = ConstantValue?.Clone();
            otherField.Value = Value.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}