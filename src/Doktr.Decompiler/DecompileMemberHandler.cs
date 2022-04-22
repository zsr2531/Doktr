using Doktr.Core;
using MediatR;

namespace Doktr.Decompiler;

public class DecompileMemberHandler : IRequestHandler<DecompileMember, string>
{
    public Task<string> Handle(DecompileMember request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}