using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models.Constraints;

public class ReferenceTypeParameterConstraint : TypeKindTypeParameterConstraint
{
    private NullabilityKind _nullability;

    public TypeSignature? BaseType { get; set; }

    public NullabilityKind Nullability
    {
        get => BaseType is null
            ? _nullability
            : _nullability = BaseType.Nullability;
        set => _nullability = BaseType is null
            ? value
            : BaseType.Nullability = value;
    }

    public override void AcceptVisitor(ITypeParameterConstraintVisitor visitor) => visitor.VisitReferenceType(this);

    public override ReferenceTypeParameterConstraint Clone() => new()
    {
        BaseType = BaseType?.Clone(),
        Nullability = Nullability
    };
}