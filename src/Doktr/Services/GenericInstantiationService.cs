using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Services;

public class GenericInstantiationService : IGenericInstantiationService
{
    private static readonly SignatureComparer Comparer = new();

    private static GenericContext CreateGenericContext(TypeSignature typeSignature)
    {
        return typeSignature is GenericInstanceTypeSignature generic
            ? new GenericContext().WithType(generic)
            : default;
    }

    public MethodSignature InstantiateMethodSignature(MethodSignature signature, TypeSignature typeSignature)
    {
        try
        {
            var context = CreateGenericContext(typeSignature);
            var instantiated = signature.InstantiateGenericTypes(context);

            return instantiated;
        }
        catch
        {
            return signature;
        }
    }

    public bool Equals(MethodSignature x, MethodSignature y, TypeSignature typeSignature)
    {
        var xInstantiated = InstantiateMethodSignature(x, typeSignature);
        var yInstantiated = InstantiateMethodSignature(y, typeSignature);

        return Comparer.Equals(xInstantiated, yInstantiated);
    }
}