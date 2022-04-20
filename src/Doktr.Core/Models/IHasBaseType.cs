namespace Doktr.Core.Models;

public interface IHasBaseType
{
    CodeReference? BaseType { get; }
}