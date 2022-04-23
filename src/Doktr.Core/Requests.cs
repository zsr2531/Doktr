using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using MediatR;

namespace Doktr.Core;

public record DecompileMember(MemberDocumentation Member) : IRequest<string>;

public record DecompileTypeSignature(TypeSignature Signature) : IRequest<string>;

public record ParseDocumentation(CodeReference MemberIdentifier, string RawXml) : IRequest<ParseDocumentationResult>;