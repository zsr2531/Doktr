using System;
using System.Linq;
using AsmResolver.DotNet.Signatures.Types;
using Serilog;

namespace Doktr.Services;

public class DocumentIdTranslatorService : IDocumentIdTranslatorService
{
    private readonly ILogger _logger;

    public DocumentIdTranslatorService(ILogger logger)
    {
        _logger = logger;
    }

    public string VisitArrayType(ArrayTypeSignature signature)
    {
        var type = signature.BaseType;
        int dimensions = signature.Dimensions.Count;
        string dimensionsFormatted = string.Join(",", Enumerable.Repeat("0:", dimensions));

        return $"{type.AcceptVisitor(this)}[{dimensionsFormatted}]";
    }

    public string VisitBoxedType(BoxedTypeSignature signature) => throw new NotSupportedException();

    public string VisitByReferenceType(ByReferenceTypeSignature signature)
    {
        return $"{signature.BaseType.AcceptVisitor(this)}@";
    }

    public string VisitCorLibType(CorLibTypeSignature signature)
    {
        return signature.Type.FullName;
    }

    public string VisitCustomModifierType(CustomModifierTypeSignature signature)
    {
        return $"{signature.BaseType.AcceptVisitor(this)}";
    }

    public string VisitGenericInstanceType(GenericInstanceTypeSignature signature)
    {
        string name = signature.GenericType.FullName;
        int backTick = name.IndexOf('`');
        string realName = name[..backTick];
        var parameters = signature.TypeArguments.Select(a => a.AcceptVisitor(this));
        string parametersFormatted = string.Join("", parameters)
            .Replace('<', '{')
            .Replace('>', '}');

        return realName + '{' + parametersFormatted + '}';
    }

    public string VisitGenericParameter(GenericParameterSignature signature)
    {
        return signature.Name.Replace('!', '`');
    }

    public string VisitPinnedType(PinnedTypeSignature signature) => throw new NotSupportedException();

    public string VisitPointerType(PointerTypeSignature signature)
    {
        return $"{signature.BaseType.AcceptVisitor(this)}*";
    }

    public string VisitSentinelType(SentinelTypeSignature signature) => throw new NotSupportedException();

    public string VisitSzArrayType(SzArrayTypeSignature signature)
    {
        return $"{signature.BaseType.AcceptVisitor(this)}[]";
    }

    public string VisitTypeDefOrRef(TypeDefOrRefSignature signature)
    {
        return signature.Type.FullName;
    }

    public string VisitFunctionPointerType(FunctionPointerTypeSignature signature)
    {
        _logger.Warning("Function pointers are currently not supported in xmldoc");
        return "";
    }
}