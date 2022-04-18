namespace Doktr.Core.Models;

public interface IHasVirtual
{
    bool IsVirtual { get; set; }
    bool IsOverride { get; set; }
    bool IsSealed { get; set; }
}