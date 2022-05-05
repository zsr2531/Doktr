namespace Doktr.Core.Models.Constraints;

public interface ITypeParameterConstraintVisitor
{
    void VisitReferenceType(ReferenceTypeParameterConstraint constraint);

    void VisitValueType(ValueTypeParameterConstraint constraint);

    void VisitNotNullType(NotNullTypeKindTypeParameterConstraint constraint);

    void VisitDefaultType(DefaultTypeKindTypeParameterConstraint constraint);

    void VisitInterface(InterfaceTypeParameterConstraint constraint);

    void VisitConstructor(ConstructorTypeParameterConstraint constraint);
}