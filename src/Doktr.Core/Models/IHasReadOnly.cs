namespace Doktr.Core.Models;

public interface IHasReadOnly
{
    bool IsReadOnly { get; set; }
}