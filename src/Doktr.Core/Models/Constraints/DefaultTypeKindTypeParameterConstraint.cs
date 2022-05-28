namespace Doktr.Core.Models.Constraints;

public class DefaultTypeKindTypeParameterConstraint : TypeKindTypeParameterConstraint
{
    public override DefaultTypeKindTypeParameterConstraint Clone() => new();

    public override void AcceptVisitor(ITypeParameterConstraintVisitor visitor) => visitor.VisitDefaultType(this);
}