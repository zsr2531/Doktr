using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

internal interface ITypeSignatureDecompilationStrategy : ITypeSignatureVisitor
{
    string ToString();
}