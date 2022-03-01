using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models;

public class ConstructorDocumentation : IMemberDocumentation, IHasExceptions
{
    public ConstructorDocumentation(string name)
    {
        Name = name;
    }

    public ConstructorDocumentation(ConstructorDocumentation other)
    {
        Name = other.Name;
        Summary = other.Summary;
        Syntax = other.Syntax;
        Parameters = other.Parameters;
        Exceptions = other.Exceptions;
        Examples = other.Examples;
        Remarks = other.Remarks;
        SeeAlso = other.SeeAlso;
    }

    public string Name { get; }

    public ImmutableArray<IDocumentationSegment> Summary { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public string? Syntax { get; init; }

    public ImmutableArray<ParameterDocumentation> Parameters { get; init; } =
        ImmutableArray<ParameterDocumentation>.Empty;

    public ImmutableArray<ExceptionDocumentation> Exceptions { get; init; } =
        ImmutableArray<ExceptionDocumentation>.Empty;

    public ImmutableArray<IDocumentationSegment> Examples { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public ImmutableArray<IDocumentationSegment> Remarks { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public ImmutableArray<IReference> SeeAlso { get; init; } = ImmutableArray<IReference>.Empty;
}