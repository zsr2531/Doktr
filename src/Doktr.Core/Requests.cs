using Doktr.Core.Models;
using MediatR;

namespace Doktr.Core;

public record DecompileMember(MemberDocumentation Member) : IRequest<string>;

public record ParseDocumentation(CodeReference MemberIdentifier, string RawXml) : IRequest<ParseDocumentationResult>;