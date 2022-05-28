using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models.Constraints;

public class InterfaceTypeParameterConstraint : TypeParameterConstraint
{
    public InterfaceTypeParameterConstraint(TypeSignature interfaceType)
    {
        InterfaceType = interfaceType;
    }

    public TypeSignature InterfaceType { get; set; }

    public override void AcceptVisitor(ITypeParameterConstraintVisitor visitor) => visitor.VisitInterface(this);

    public override InterfaceTypeParameterConstraint Clone() => new(InterfaceType.Clone());
}