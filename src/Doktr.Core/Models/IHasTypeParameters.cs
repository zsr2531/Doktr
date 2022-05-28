using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasTypeParameters
{
    TypeParameterDocumentationCollection TypeParameters { get; }
}