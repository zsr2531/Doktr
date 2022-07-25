using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Lifters.AsmResolver.Utils;

public interface ITypeSignatureTranslator : ITypeSignatureVisitor<string>
{
}