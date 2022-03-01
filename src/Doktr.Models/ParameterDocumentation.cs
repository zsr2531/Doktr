using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models;

public class ParameterDocumentation
{
    public ParameterDocumentation(string name, IReference type)
    {
        Name = name;
        Type = type;
    }

    public ParameterDocumentation(ParameterDocumentation other)
    {
        Name = other.Name;
        Type = other.Type;
        Modifier = other.Modifier;
        IsParams = other.IsParams;
        IsOptional = other.IsOptional;
        DefaultValue = other.DefaultValue;
        Documentation = other.Documentation;
    }

    public string Name { get; }

    public IReference Type { get; init; }

    public ParameterModifier Modifier { get; init; } = ParameterModifier.None;

    public bool IsParams { get; init; } = false;

    public bool IsOptional { get; init; } = false;

    public object? DefaultValue { get; init; } = null;

    public ImmutableArray<IDocumentationSegment> Documentation { get; init; } =
        ImmutableArray<IDocumentationSegment>.Empty;
}