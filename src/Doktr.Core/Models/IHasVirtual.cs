namespace Doktr.Core.Models;

public interface IHasVirtual : IHasAbstract
{
    bool IsVirtual { get; }
    bool IsOverride { get; }
    CodeReference? Overrides { get; }
}