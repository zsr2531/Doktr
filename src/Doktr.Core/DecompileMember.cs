using Doktr.Core.Models;
using MediatR;

namespace Doktr.Core;

public class DecompileMember : IRequest<string>
{
    public DecompileMember(MemberDocumentation member)
    {
        Member = member;
    }

    public MemberDocumentation Member { get; }
}