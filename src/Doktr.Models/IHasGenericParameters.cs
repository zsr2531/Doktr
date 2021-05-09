using System.Collections.Immutable;

namespace Doktr.Models
{
    public interface IHasGenericParameters
    {
        ImmutableArray<GenericParameterDocumentation> GenericParameters
        {
            get;
        }
    }
}