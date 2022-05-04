namespace Doktr.Core.Models.Signatures;

public interface ITypeSignatureVisitor
{
    void VisitVanilla(VanillaTypeSignature vanillaTypeSignature);
    
    void VisitGenericInstance(GenericInstanceTypeSignature genericInstanceTypeSignature);

    void VisitGenericParameter(GenericParameterTypeSignature genericParameterTypeSignature);

    void VisitSzArray(SzArrayTypeSignature szArrayTypeSignature);

    void VisitNullableValue(NullableValueTypeSignature nullableValueTypeSignature);

    void VisitValueTuple(ValueTupleTypeSignature valueTupleTypeSignature);

    void VisitPointer(PointerTypeSignature pointerTypeSignature);

    void VisitJaggedArray(JaggedArrayTypeSignature jaggedArrayTypeSignature);

    void VisitFunctionPointer(FunctionPointerTypeSignature functionPointerTypeSignature);
}