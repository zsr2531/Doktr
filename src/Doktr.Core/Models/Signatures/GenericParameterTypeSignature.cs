namespace Doktr.Core.Models.Signatures;

public class GenericParameterTypeSignature : TypeSignature
{
    public GenericParameterTypeSignature(string name)
        : base(name)
    {
    }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitGenericParameter(this);

    public override GenericParameterTypeSignature Clone() => new(Name);
}