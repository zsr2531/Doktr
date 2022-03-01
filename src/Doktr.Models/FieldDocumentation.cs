using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models;

public class FieldDocumentation : IMemberDocumentation
{
    public FieldDocumentation(string name, IReference type)
    {
        Name = name;
        Type = type;
    }

    public FieldDocumentation(FieldDocumentation other)
    {
        Name = other.Name;
        Type = other.Type;
        Summary = other.Summary;
        Syntax = other.Syntax;
        Examples = other.Examples;
        Remarks = other.Remarks;
        SeeAlso = other.SeeAlso;
    }

    public string Name { get; }

    public IReference Type { get; init; }

    public ImmutableArray<IDocumentationSegment> Summary { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public string? Syntax { get; init; }

    public ImmutableArray<IDocumentationSegment> Examples { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public ImmutableArray<IDocumentationSegment> Remarks { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public ImmutableArray<IReference> SeeAlso { get; init; } = ImmutableArray<IReference>.Empty;
}