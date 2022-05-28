namespace Doktr.Core.Models.Signatures;

public class NullableValueTypeSignature : TypeSignature
{
    public NullableValueTypeSignature(TypeSignature valueType)
    {
        ValueType = valueType;
    }

    public TypeSignature ValueType { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitNullableValue(this);

    public override NullableValueTypeSignature Clone() => new(ValueType.Clone());
}