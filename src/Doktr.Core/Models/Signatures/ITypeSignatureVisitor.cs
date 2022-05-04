namespace Doktr.Core.Models.Signatures;

public interface ITypeSignatureVisitor
{
    void VisitVanilla(VanillaTypeSignature signature);
    
    void VisitGenericInstance(GenericInstanceTypeSignature signature);

    void VisitGenericParameter(GenericParameterTypeSignature signature);

    void VisitSzArray(SzArrayTypeSignature signature);

    void VisitNullableValue(NullableValueTypeSignature signature);

    void VisitValueTuple(ValueTupleTypeSignature signature);

    void VisitPointer(PointerTypeSignature signature);

    void VisitJaggedArray(JaggedArrayTypeSignature signature);

    void VisitFunctionPointer(FunctionPointerTypeSignature signature);
}