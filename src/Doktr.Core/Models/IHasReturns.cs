using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public interface IHasReturns
{
    TypeSignature ReturnType { get; }
    DocumentationFragmentCollection Returns { get; }
}