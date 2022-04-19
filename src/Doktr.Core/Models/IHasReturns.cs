using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public interface IHasReturns
{
    DocumentationFragmentCollection Returns { get; }
}