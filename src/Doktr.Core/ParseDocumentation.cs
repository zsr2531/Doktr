using Doktr.Core.Models;
using MediatR;

namespace Doktr.Core;

public class ParseDocumentation : IRequest<ParseDocumentationResult>
{
    public ParseDocumentation(CodeReference memberIdentifier, string rawXml)
    {
        MemberIdentifier = memberIdentifier;
        RawXml = rawXml;
    }

    public CodeReference MemberIdentifier { get; }
    public string RawXml { get; }
}