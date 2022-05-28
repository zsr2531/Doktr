namespace Doktr.Core.Models.Signatures;

public abstract class TypeSignature : ICloneable
{
    public NullabilityKind Nullability { get; set; } = NullabilityKind.NullOblivious;

    public abstract void AcceptVisitor(ITypeSignatureVisitor visitor);

    public abstract TypeSignature Clone();

    object ICloneable.Clone() => Clone();
}