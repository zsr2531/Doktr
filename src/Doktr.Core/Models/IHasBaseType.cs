using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public interface IHasBaseType
{
    TypeSignature? BaseType { get; }
}