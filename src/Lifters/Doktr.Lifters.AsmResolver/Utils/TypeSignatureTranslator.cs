using System.Text.RegularExpressions;
using AsmResolver.DotNet.Signatures.Types;
using Serilog;

namespace Doktr.Lifters.AsmResolver.Utils;

public class TypeSignatureTranslator : ITypeSignatureTranslator
{
    private static readonly Regex GenericPostFixRegex = new(@"`(\d+)", RegexOptions.Compiled);

    private readonly ILogger _logger;

    public TypeSignatureTranslator(ILogger logger)
    {
        _logger = logger;
    }

    public string VisitArrayType(ArrayTypeSignature signature)
    {
        string arrayType = signature.BaseType.AcceptVisitor(this);
        int dimensions = signature.Dimensions.Count;
        string dims = string.Join(',', Enumerable.Repeat("0:", dimensions - 1));

        return $"{arrayType}[{dims}]";
    }

    public string VisitBoxedType(BoxedTypeSignature signature) => throw new NotSupportedException();

    public string VisitByReferenceType(ByReferenceTypeSignature signature)
    {
        string baseType = signature.BaseType.AcceptVisitor(this);
        return $"{baseType}@";
    }

    public string VisitCorLibType(CorLibTypeSignature signature) => SwapPlus(signature.Type.FullName);

    public string VisitCustomModifierType(CustomModifierTypeSignature signature) =>
        signature.BaseType.AcceptVisitor(this);

    public string VisitGenericInstanceType(GenericInstanceTypeSignature signature)
    {
        string baseType = SwapPlus(signature.GenericType.FullName);
        var typeArguments = signature.TypeArguments.Select(s => s.AcceptVisitor(this)).ToList();

        return GenericPostFixRegex.Replace(baseType, match =>
        {
            int count = int.Parse(match.Groups[1].Value);
            string parameters = string.Join(',', typeArguments.Take(count));
            typeArguments.RemoveRange(0, count);

            return $"{{{parameters}}}";
        });
    }

    public string VisitGenericParameter(GenericParameterSignature signature) => signature.Name.Replace('!', '`');

    public string VisitPinnedType(PinnedTypeSignature signature) => throw new NotSupportedException();

    public string VisitPointerType(PointerTypeSignature signature)
    {
        string baseType = signature.BaseType.AcceptVisitor(this);
        return $"{baseType}*";
    }

    public string VisitSentinelType(SentinelTypeSignature signature) => throw new NotSupportedException();

    public string VisitSzArrayType(SzArrayTypeSignature signature)
    {
        string arrayType = signature.BaseType.AcceptVisitor(this);
        return $"{arrayType}[]";
    }

    public string VisitTypeDefOrRef(TypeDefOrRefSignature signature) => SwapPlus(signature.Type.FullName);

    public string VisitFunctionPointerType(FunctionPointerTypeSignature signature)
    {
        _logger.Warning("Function pointers are not supported in xmldoc yet, documentation mapping may fail if" +
            "you have multiple overloads");
        return "";
    }

    private static string SwapPlus(string name) => name.Replace('+', '.');
}