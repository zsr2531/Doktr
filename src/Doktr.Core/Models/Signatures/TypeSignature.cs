namespace Doktr.Core.Models.Signatures;

public abstract class TypeSignature : ICloneable
{
    protected TypeSignature(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public NullabilityKind Nullability { get; set; } = NullabilityKind.NullOblivious;

    public abstract void AcceptVisitor(ITypeSignatureVisitor visitor);

    public abstract TypeSignature Clone();

    object ICloneable.Clone() => Clone();
}