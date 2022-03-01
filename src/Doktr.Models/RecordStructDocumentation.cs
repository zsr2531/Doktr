using System.Collections.Immutable;
using Doktr.Models.References;

namespace Doktr.Models;

public class RecordStructDocumentation : TypeDocumentation
{
    public RecordStructDocumentation(
        string assembly,
        string ns,
        string name,
        ImmutableArray<ParameterDocumentation> elements)
        : base(assembly, ns, name)
    {
        Elements = elements;
    }

    public RecordStructDocumentation(RecordStructDocumentation other)
        : base(other)
    {
        Elements = other.Elements;
        Implementations = other.Implementations;
    }

    public ImmutableArray<ParameterDocumentation> Elements { get; }

    public ImmutableArray<IReference> Implementations { get; init; } = ImmutableArray<IReference>.Empty;
}