using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasExtensionMethods
{
    ExtensionMethodCollection ExtensionMethods { get; set; }
}