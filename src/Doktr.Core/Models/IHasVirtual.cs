namespace Doktr.Core.Models;

public interface IHasVirtual : IHasAbstract
{
    bool IsVirtual { get; set; }
    bool IsOverride { get; set; }
}