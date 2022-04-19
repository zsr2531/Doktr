namespace Doktr.Core.Models;

public interface IHasAbstract
{
    bool IsAbstract { get; set; }
    bool IsSealed { get; set; }
}