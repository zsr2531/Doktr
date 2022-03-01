using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models;

public class MethodDocumentation : IMemberDocumentation, IHasExceptions, IHasGenericParameters
{
    public MethodDocumentation(string name, IReference returnType)
    {
        Name = name;
        ReturnType = returnType;
    }

    public MethodDocumentation(MethodDocumentation other)
    {
        Name = other.Name;
        Parameters = other.Parameters;
        ReturnType = other.ReturnType;
        Summary = other.Summary;
        Syntax = other.Syntax;
        Examples = other.Examples;
        Remarks = other.Remarks;
        SeeAlso = other.SeeAlso;
    }

    public string Name { get; }

    public ImmutableArray<IDocumentationSegment> Summary { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public string? Syntax { get; init; }

    public ImmutableArray<GenericParameterDocumentation> GenericParameters { get; init; } =
        ImmutableArray<GenericParameterDocumentation>.Empty;

    public ImmutableArray<ParameterDocumentation> Parameters { get; init; } =
        ImmutableArray<ParameterDocumentation>.Empty;

    public IReference ReturnType { get; init; }

    public ImmutableArray<IDocumentationSegment> Returns { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public ImmutableArray<ExceptionDocumentation> Exceptions { get; init; } =
        ImmutableArray<ExceptionDocumentation>.Empty;

    public ImmutableArray<IDocumentationSegment> Examples { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public ImmutableArray<IDocumentationSegment> Remarks { get; init; } = ImmutableArray<IDocumentationSegment>.Empty;

    public ImmutableArray<IReference> SeeAlso { get; init; } = ImmutableArray<IReference>.Empty;
}