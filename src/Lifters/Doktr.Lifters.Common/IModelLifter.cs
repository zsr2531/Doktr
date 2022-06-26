using Doktr.Core;
using Doktr.Core.Models.Collections;

namespace Doktr.Lifters.Common;

public interface IModelLifter
{
    AssemblyTypesMap LiftModels(IEnumerable<DoktrTarget> targets);
}