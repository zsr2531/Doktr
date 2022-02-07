using System;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Services;

public interface ITypeSignatureSourceService
{
    NullableContext WithContext(IHasCustomAttribute? ca);

    void PushNullableContext(IHasCustomAttribute? ca);

    void PopNullableContext();

    string GetSource(
        IHasCustomAttribute? member,
        TypeSignature signature,
        NullabilityAwareTypeSignatureVisitor.GetGenericParameterInfo gpInfo,
        bool dropRef = false);

    public class NullableContext : IDisposable
    {
        private readonly ITypeSignatureSourceService _typeSignatureSourceService;

        public NullableContext(ITypeSignatureSourceService typeSignatureSourceService, IHasCustomAttribute? ca)
        {
            _typeSignatureSourceService = typeSignatureSourceService;
            typeSignatureSourceService.PushNullableContext(ca);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _typeSignatureSourceService.PopNullableContext();
        }
    }
}