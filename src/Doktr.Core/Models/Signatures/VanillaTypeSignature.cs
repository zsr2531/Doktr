namespace Doktr.Core.Models.Signatures;

public class VanillaTypeSignature : TypeSignature
{
    public VanillaTypeSignature(string name, CodeReference type)
        : base(name)
    {
        Type = type;
    }
    
    public CodeReference Type { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitVanilla(this);

    public override VanillaTypeSignature Clone() => new(Name, Type);
}