using System.Text;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

public class TypeSignatureDecompilationStrategy : ITypeSignatureVisitor
{
    protected readonly StringBuilder _sb = new();

    public virtual void VisitVanilla(VanillaTypeSignature vanillaTypeSignature)
    {
        _sb.Append(vanillaTypeSignature.Name);
    }

    public virtual void VisitGenericInstance(GenericInstanceTypeSignature genericInstanceTypeSignature)
    {
        genericInstanceTypeSignature.GenericType.AcceptVisitor(this);
        _sb.Append('<');

        var typeArgs = genericInstanceTypeSignature.TypeArguments;
        for (int i = 0; i < typeArgs.Count; i++)
        {
            var current = typeArgs[i];
            current.AcceptVisitor(this);

            if (i + 1 < typeArgs.Count)
                _sb.Append(", ");
        }

        _sb.Append('>');
    }

    public virtual void VisitGenericParameter(GenericParameterTypeSignature genericParameterTypeSignature)
    {
        _sb.Append(genericParameterTypeSignature.Name);
    }

    public override string ToString() => _sb.ToString();
}