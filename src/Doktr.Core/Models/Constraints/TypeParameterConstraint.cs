namespace Doktr.Core.Models.Constraints;

public abstract class TypeParameterConstraint : ICloneable
{
    public abstract TypeParameterConstraint Clone();

    public abstract void AcceptVisitor(ITypeParameterConstraintVisitor visitor);

    object ICloneable.Clone() => Clone();
}

public abstract class TypeKindTypeParameterConstraint : TypeParameterConstraint
{
    public abstract override TypeKindTypeParameterConstraint Clone();
}