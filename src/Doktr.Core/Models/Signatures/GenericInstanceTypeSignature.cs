using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Signatures;

public class GenericInstanceTypeSignature : TypeSignature
{
    public GenericInstanceTypeSignature(TypeSignature genericType)
    {
        GenericType = genericType;
    }

    public TypeSignature GenericType { get; set; }
    public TypeSignatureCollection TypeParameters { get; set; } = new();

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitGenericInstance(this);

    public override GenericInstanceTypeSignature Clone() => new(GenericType.Clone())
    {
        TypeParameters = TypeParameters.Clone()
    };
}