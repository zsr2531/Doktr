using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Services;

public interface ITypeSignatureTranslationService : ITypeSignatureVisitor<string>
{
}