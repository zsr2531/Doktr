using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasExtensionMethods
{
    MethodCodeReferenceCollection ExtensionMethods { get; }
}