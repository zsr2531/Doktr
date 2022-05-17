using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;
using MediatR;

namespace Doktr.Core;

[ExcludeFromCodeCoverage]
public record DecompileMember(MemberDocumentation Member) : IRequest<string>;

[ExcludeFromCodeCoverage]
public record DecompileTypeSignature(TypeSignature Signature) : IRequest<string>;

[ExcludeFromCodeCoverage]
public record ParseDocumentation(CodeReference MemberIdentifier, string RawXml) : IRequest<ParseDocumentationResult>;

[ExcludeFromCodeCoverage]
public record LiftModel : IRequest<TypeDocumentationCollection>;