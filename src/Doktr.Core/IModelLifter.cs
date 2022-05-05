using Doktr.Core.Models.Collections;

namespace Doktr.Core;

public interface IModelLifter
{
    TypeDocumentationCollection LiftModels();
}