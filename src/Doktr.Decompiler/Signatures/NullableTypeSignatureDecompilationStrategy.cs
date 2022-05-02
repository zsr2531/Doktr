using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

public class NullableTypeSignatureDecompilationStrategy : TypeSignatureDecompilationStrategy
{
    public override void VisitVanilla(VanillaTypeSignature vanillaTypeSignature)
    {
        base.VisitVanilla(vanillaTypeSignature);
        WriteNullability(vanillaTypeSignature);
    }

    public override void VisitGenericInstance(GenericInstanceTypeSignature genericInstanceTypeSignature)
    {
        base.VisitGenericInstance(genericInstanceTypeSignature);
        WriteNullability(genericInstanceTypeSignature);
    }

    public override void VisitGenericParameter(GenericParameterTypeSignature genericParameterTypeSignature)
    {
        base.VisitGenericParameter(genericParameterTypeSignature);
        WriteNullability(genericParameterTypeSignature);
    }

    public override void VisitSzArray(SzArrayTypeSignature szArrayTypeSignature)
    {
        base.VisitSzArray(szArrayTypeSignature);
        WriteNullability(szArrayTypeSignature);
    }

    private void WriteNullability(TypeSignature typeSignature)
    {
        if (typeSignature.Nullability == NullabilityKind.Nullable)
            Builder.Append('?');
    }
}