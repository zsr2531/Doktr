using Serilog;

namespace Doktr.Services;

using System;
using System.Linq;
using AsmResolver.DotNet.Signatures.Types;

public class TypeSignatureTranslationService : ITypeSignatureTranslationService
{
    private readonly ILogger _logger;

    public TypeSignatureTranslationService(ILogger logger)
    {
        _logger = logger;
    }

    public string VisitArrayType(ArrayTypeSignature signature)
    {
        var type = signature.BaseType;
        int dimensions = signature.Dimensions.Count;

        return $"{type.AcceptVisitor(this)}[{string.Join(",", Enumerable.Repeat("0:", dimensions))}]";
    }

    public string VisitBoxedType(BoxedTypeSignature signature) => throw new NotSupportedException();

    public string VisitByReferenceType(ByReferenceTypeSignature signature) => $"{signature.BaseType.AcceptVisitor(this)}@";

    public string VisitCorLibType(CorLibTypeSignature signature) => signature.Type.FullName;

    public string VisitCustomModifierType(CustomModifierTypeSignature signature) => signature.BaseType.AcceptVisitor(this);

    public string VisitGenericInstanceType(GenericInstanceTypeSignature signature)
    {
        string name = signature.GenericType.FullName;
        int indexOfBackTick = name.IndexOf('`');
        string realName = name[..indexOfBackTick];
        string parameters = string.Join(',', signature.TypeArguments.Select(a => a.AcceptVisitor(this)))
            .Replace('<', '{').Replace('>', '}').Replace('!', '`');

        return realName + '{' + parameters + '}';
    }

    public string VisitGenericParameter(GenericParameterSignature signature) => signature.Name.Replace('!', '`');

    public string VisitPinnedType(PinnedTypeSignature signature) => throw new NotSupportedException();

    public string VisitPointerType(PointerTypeSignature signature) => $"{signature.BaseType.AcceptVisitor(this)}*";

    public string VisitSentinelType(SentinelTypeSignature signature) => throw new NotSupportedException();

    public string VisitSzArrayType(SzArrayTypeSignature signature) => $"{signature.BaseType.AcceptVisitor(this)}[]";

    public string VisitTypeDefOrRef(TypeDefOrRefSignature signature) => signature.Type.FullName;

    public string VisitFunctionPointerType(FunctionPointerTypeSignature signature)
    {
        _logger.Warning("Function pointers are not supported in xmldoc yet");
        return "";
    }
}