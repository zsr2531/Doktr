namespace Doktr.Core.Models.Signatures;

public class SzArrayTypeSignature : TypeSignature
{
    public SzArrayTypeSignature(TypeSignature arrayType)
    {
        ArrayType = arrayType;
    }

    public TypeSignature ArrayType { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitSzArray(this);

    public override SzArrayTypeSignature Clone() => new(ArrayType.Clone());
}