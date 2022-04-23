using System.Text;
using Doktr.Core;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

internal class NullableTypeSignatureDecompilationStrategy : ITypeSignatureDecompilationStrategy
{
    private readonly StringBuilder _sb = new();
    private readonly DoktrConfiguration _configuration;

    internal NullableTypeSignatureDecompilationStrategy(DoktrConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void VisitVanilla(VanillaTypeSignature vanillaTypeSignature)
    {
        _sb.Append(vanillaTypeSignature.Name);
        AddNullabilityTag(vanillaTypeSignature);
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
        AddNullabilityTag(genericInstanceTypeSignature);
    }

    public void VisitGenericParameter(GenericParameterTypeSignature genericParameterTypeSignature)
    {
        _sb.Append(genericParameterTypeSignature.Name);
        AddNullabilityTag(genericParameterTypeSignature);
    }

    public override string ToString() => _sb.ToString();

    private void AddNullabilityTag(TypeSignature signature)
    {
        if (_configuration.EnableNrt && signature.Nullability == NullabilityKind.Nullable)
            _sb.Append('?');
    }
}