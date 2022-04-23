using System.Diagnostics.CodeAnalysis;

namespace Doktr.Core.Models;

public interface IHasVirtual : IHasAbstract
{
    bool IsVirtual { get; }
    [MemberNotNullWhen(true, nameof(Overrides))]
    bool IsOverride { get; }
    CodeReference? Overrides { get; }
}