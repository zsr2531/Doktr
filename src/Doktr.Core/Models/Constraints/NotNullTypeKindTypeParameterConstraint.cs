namespace Doktr.Core.Models.Constraints;

public class NotNullTypeKindTypeParameterConstraint : TypeKindTypeParameterConstraint
{
    public override NotNullTypeKindTypeParameterConstraint Clone() => new();

    public override void AcceptVisitor(ITypeParameterConstraintVisitor visitor) => visitor.VisitNotNullType(this);
}