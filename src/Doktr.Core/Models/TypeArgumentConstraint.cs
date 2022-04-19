using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public abstract class TypeArgumentConstraint
{
}

public abstract class TypeKindTypeArgumentConstraint : TypeArgumentConstraint
{
}

public class ReferenceTypeArgumentConstraint : TypeKindTypeArgumentConstraint
{
    public TypeSignature? BaseType { get; set; } = null;
    public NullabilityKind Nullability { get; set; } = NullabilityKind.NotNullable;
}

public class ValueTypeArgumentConstraint : TypeKindTypeArgumentConstraint
{
}

public class InterfaceTypeArgumentConstraint : TypeArgumentConstraint
{
    public InterfaceTypeArgumentConstraint(TypeSignature interfaceType)
    {
        InterfaceType = interfaceType;
    }

    public TypeSignature InterfaceType { get; set; }
    public NullabilityKind Nullability { get; set; } = NullabilityKind.NotNullable;
}