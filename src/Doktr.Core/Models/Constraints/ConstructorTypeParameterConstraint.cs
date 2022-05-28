namespace Doktr.Core.Models.Constraints;

public class ConstructorTypeParameterConstraint : TypeParameterConstraint
{
    public override ConstructorTypeParameterConstraint Clone() => new();

    public override void AcceptVisitor(ITypeParameterConstraintVisitor visitor) => visitor.VisitConstructor(this);
}