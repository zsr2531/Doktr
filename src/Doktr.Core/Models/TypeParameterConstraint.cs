using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public abstract class TypeParameterConstraint : ICloneable
{
    public abstract TypeParameterConstraint Clone();

    object ICloneable.Clone() => Clone();
}

public abstract class TypeKindTypeParameterConstraint : TypeParameterConstraint
{
    public abstract override TypeKindTypeParameterConstraint Clone();
}

public class ReferenceTypeParameterConstraint : TypeKindTypeParameterConstraint
{
    public TypeSignature? BaseType { get; set; }
    // Note that this only has meaning if BaseType is null.
    public NullabilityKind Nullability { get; set; }

    public override ReferenceTypeParameterConstraint Clone() => new()
    {
        BaseType = BaseType?.Clone(),
        Nullability = Nullability
    };
}

public class ValueTypeParameterConstraint : TypeKindTypeParameterConstraint
{
    public bool IsUnmanaged { get; set; }
    
    public override ValueTypeParameterConstraint Clone() => new()
    {
        IsUnmanaged = IsUnmanaged
    };
}

public class InterfaceTypeParameterConstraint : TypeParameterConstraint
{
    public InterfaceTypeParameterConstraint(TypeSignature interfaceType)
    {
        InterfaceType = interfaceType;
    }

    public TypeSignature InterfaceType { get; set; }

    public override InterfaceTypeParameterConstraint Clone() => new(InterfaceType.Clone());
}