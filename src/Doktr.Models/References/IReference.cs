using System.Collections.Immutable;

namespace Doktr.Models.References;

public interface IReference
{
    string Cref { get; }

    ImmutableArray<IReference> GenericParameters { get; }
}