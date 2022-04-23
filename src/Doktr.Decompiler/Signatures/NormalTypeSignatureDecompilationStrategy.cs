using System.Text;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

internal class NormalTypeSignatureDecompilationStrategy : ITypeSignatureDecompilationStrategy
{
    private readonly StringBuilder _sb = new();

    public void VisitVanilla(VanillaTypeSignature vanillaTypeSignature)
    {
        _sb.Append(vanillaTypeSignature.Name);
    }

    public void VisitGenericInstance(GenericInstanceTypeSignature genericInstanceTypeSignature)
    {
        genericInstanceTypeSignature.GenericType.AcceptVisitor(this);
        _sb.Append('<');
        var args = genericInstanceTypeSignature.TypeArguments;

        for (int i = 0; i < args.Count; i++)
        {
            var current = args[i];
            current.AcceptVisitor(this);

            if (i + 1 < args.Count)
                _sb.Append(", ");
        }

        _sb.Append('>');
    }

    public void VisitGenericParameter(GenericParameterTypeSignature genericParameterTypeSignature)
    {
        _sb.Append(genericParameterTypeSignature.Name);
    }

    public override string ToString() => _sb.ToString();
}