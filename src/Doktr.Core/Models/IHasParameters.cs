using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasParameters
{
    ParameterDocumentationCollection Parameters { get; }
}