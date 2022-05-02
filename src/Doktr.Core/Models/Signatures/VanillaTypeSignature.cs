namespace Doktr.Core.Models.Signatures;

public class VanillaTypeSignature : TypeSignature
{
    public VanillaTypeSignature(CodeReference type)
    {
        Type = type;
    }
    
    public CodeReference Type { get; set; }

    public override void AcceptVisitor(ITypeSignatureVisitor visitor) => visitor.VisitVanilla(this);

    public override VanillaTypeSignature Clone() => new(Type);
}