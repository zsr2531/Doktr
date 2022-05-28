using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

public class NullableTypeSignatureDecompilationStrategy : TypeSignatureDecompilationStrategy
{
    public override void VisitVanilla(VanillaTypeSignature signature)
    {
        base.VisitVanilla(signature);
        WriteNullability(signature);
    }

    public override void VisitGenericInstance(GenericInstanceTypeSignature signature)
    {
        base.VisitGenericInstance(signature);
        WriteNullability(signature);
    }

    public override void VisitGenericParameter(GenericParameterTypeSignature signature)
    {
        base.VisitGenericParameter(signature);
        WriteNullability(signature);
    }

    public override void VisitSzArray(SzArrayTypeSignature signature)
    {
        base.VisitSzArray(signature);
        WriteNullability(signature);
    }

    public override void VisitJaggedArray(JaggedArrayTypeSignature signature)
    {
        base.VisitJaggedArray(signature);
        WriteNullability(signature);
    }

    private void WriteNullability(TypeSignature typeSignature)
    {
        if (typeSignature.Nullability == NullabilityKind.Nullable)
            Builder.Append('?');
    }
}