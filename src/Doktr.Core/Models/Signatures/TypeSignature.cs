namespace Doktr.Core.Models.Signatures;

public abstract class TypeSignature
{
    protected TypeSignature(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public NullabilityKind Nullability { get; set; } = NullabilityKind.NullOblivious;

    public abstract void AcceptVisitor(ITypeSignatureVisitor visitor);
}