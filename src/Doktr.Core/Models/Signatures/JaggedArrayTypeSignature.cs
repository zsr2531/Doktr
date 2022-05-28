namespace Doktr.Core.Models.Signatures;

public class JaggedArrayTypeSignature : TypeSignature
{
    public JaggedArrayTypeSignature(int dimensions, TypeSignature arrayType)
    {
        Dimensions = dimensions;
        ArrayType = arrayType;
    }

    public int Dimensions { get; set; }
    public TypeSignature ArrayType { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitJaggedArray(this);

    public override JaggedArrayTypeSignature Clone() => new(Dimensions, ArrayType.Clone());
}