namespace Doktr.Core.Models.Signatures;

public class PointerTypeSignature : TypeSignature
{
    public PointerTypeSignature(TypeSignature pointedToType)
    {
        PointedToType = pointedToType;
    }

    public TypeSignature PointedToType { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitPointer(this);

    public override PointerTypeSignature Clone() => new(PointedToType.Clone());
}