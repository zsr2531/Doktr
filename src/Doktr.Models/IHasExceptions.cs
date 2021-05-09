using System.Collections.Immutable;

namespace Doktr.Models
{
    public interface IHasExceptions
    {
        ImmutableArray<ExceptionDocumentation> Exceptions
        {
            get;
        }
    }
}