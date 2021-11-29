using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Services;

public interface IDocumentIdTranslatorService : ITypeSignatureVisitor<string>
{
}