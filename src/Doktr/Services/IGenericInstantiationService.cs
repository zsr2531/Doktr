using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Services
{
    public interface IGenericInstantiationService
    {
        MethodSignature InstantiateMethodSignature(MethodSignature signature, TypeSignature typeSignature);

        bool Equals(MethodSignature x, MethodSignature y, TypeSignature typeSignature);
    }
}