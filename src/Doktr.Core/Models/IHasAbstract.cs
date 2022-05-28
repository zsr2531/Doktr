namespace Doktr.Core.Models;

public interface IHasAbstract
{
    bool IsAbstract { get; }
    bool IsSealed { get; }
}