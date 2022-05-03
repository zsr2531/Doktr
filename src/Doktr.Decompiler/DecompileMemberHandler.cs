using System.Diagnostics.CodeAnalysis;
using Doktr.Core;
using Doktr.Decompiler.Members;
using MediatR;

namespace Doktr.Decompiler;

[ExcludeFromCodeCoverage]
public class DecompileMemberHandler : IRequestHandler<DecompileMember, string>
{
    private readonly DoktrConfiguration _configuration;
    private readonly IMediator _mediator;

    public DecompileMemberHandler(DoktrConfiguration configuration, IMediator mediator)
    {
        _configuration = configuration;
        _mediator = mediator;
    }

    public Task<string> Handle(DecompileMember request, CancellationToken cancellationToken)
    {
        var member = request.Member;
        var decompiler = new MemberDecompiler(_mediator);
        member.AcceptVisitor(decompiler);

        return Task.FromResult(decompiler.ToString());
    }
}