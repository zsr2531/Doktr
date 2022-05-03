namespace Doktr.Core.Models.Constraints;

public class ValueTypeParameterConstraint : TypeKindTypeParameterConstraint
{
    public bool IsUnmanaged { get; set; }

    public override void AcceptVisitor(ITypeParameterConstraintVisitor visitor) => visitor.VisitValueType(this);

    public override ValueTypeParameterConstraint Clone() => new()
    {
        IsUnmanaged = IsUnmanaged
    };
}