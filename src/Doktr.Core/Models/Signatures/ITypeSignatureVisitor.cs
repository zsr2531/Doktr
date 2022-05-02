namespace Doktr.Core.Models.Signatures;

public interface ITypeSignatureVisitor
{
    void VisitVanilla(VanillaTypeSignature vanillaTypeSignature);
    
    void VisitGenericInstance(GenericInstanceTypeSignature genericInstanceTypeSignature);

    void VisitGenericParameter(GenericParameterTypeSignature genericParameterTypeSignature);

    void VisitSzArray(SzArrayTypeSignature szArrayTypeSignature);
}