namespace Doktr.Core.Models;

public interface IHasFinalizer
{
    FinalizerDocumentation? Finalizer { get; }
}