using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public abstract class TypeArgumentConstraint : ICloneable
{
    public abstract TypeArgumentConstraint Clone();

    object ICloneable.Clone() => Clone();
}

public abstract class TypeKindTypeArgumentConstraint : TypeArgumentConstraint
{
    public abstract override TypeKindTypeArgumentConstraint Clone();
}

public class ReferenceTypeArgumentConstraint : TypeKindTypeArgumentConstraint
{
    public TypeSignature? BaseType { get; set; }
    public NullabilityKind Nullability { get; set; } = NullabilityKind.NotNullable;

    public override ReferenceTypeArgumentConstraint Clone() => new()
    {
        BaseType = BaseType?.Clone(),
        Nullability = Nullability
    };
}

public class ValueTypeArgumentConstraint : TypeKindTypeArgumentConstraint
{
    public bool IsUnmanaged { get; set; }
    
    public override ValueTypeArgumentConstraint Clone() => new()
    {
        IsUnmanaged = IsUnmanaged
    };
}

public class InterfaceTypeArgumentConstraint : TypeArgumentConstraint
{
    public InterfaceTypeArgumentConstraint(TypeSignature interfaceType)
    {
        InterfaceType = interfaceType;
    }

    public TypeSignature InterfaceType { get; set; }
    public NullabilityKind Nullability { get; set; } = NullabilityKind.NotNullable;

    public override InterfaceTypeArgumentConstraint Clone() => new(InterfaceType.Clone())
    {
        Nullability = Nullability
    };
}