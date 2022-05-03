namespace Doktr.Core.Models.Constraints;

public interface ITypeParameterConstraintVisitor
{
    void VisitReferenceType(ReferenceTypeParameterConstraint constraint);

    void VisitValueType(ValueTypeParameterConstraint constraint);

    void VisitInterface(InterfaceTypeParameterConstraint constraint);
}