using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Signatures;

public class GenericInstanceTypeSignature : TypeSignature
{
    public GenericInstanceTypeSignature(TypeSignature genericType)
        : base(genericType.Name)
    {
        GenericType = genericType;
    }

    public TypeSignature GenericType { get; set; }
    public TypeSignatureCollection TypeArguments { get; set; } = new();

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitGenericInstance(this);
}