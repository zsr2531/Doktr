using Doktr.Core.Models.Collections;

namespace Doktr.Lifters.Common;

public interface IModelLifter
{
    TypeDocumentationCollection LiftModels();
}