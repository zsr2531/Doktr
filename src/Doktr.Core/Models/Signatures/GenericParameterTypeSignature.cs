namespace Doktr.Core.Models.Signatures;

public class GenericParameterTypeSignature : TypeSignature
{
    public GenericParameterTypeSignature(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitGenericParameter(this);

    public override GenericParameterTypeSignature Clone() => new(Name);
}