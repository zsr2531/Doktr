using System.Collections.Immutable;
using Doktr.Models.Segments;

namespace Doktr.Services;

public class XmlDocEntry
{
    public XmlDocEntry(
        string? inheritFrom,
        ImmutableArray<IDocumentationSegment> summary,
        ImmutableDictionary<string, ImmutableArray<IDocumentationSegment>> parameters,
        ImmutableDictionary<string, ImmutableArray<IDocumentationSegment>> typeParameters,
        ImmutableDictionary<string, ImmutableArray<IDocumentationSegment>> exceptions,
        ImmutableArray<IDocumentationSegment> returns,
        ImmutableArray<IDocumentationSegment> examples,
        ImmutableArray<IDocumentationSegment> remarks,
        ImmutableArray<string> seealso)
    {
        InheritFrom = inheritFrom;
        Summary = summary;
        Parameters = parameters;
        TypeParameters = typeParameters;
        Exceptions = exceptions;
        Returns = returns;
        Examples = examples;
        Remarks = remarks;
        Seealso = seealso;
    }

    public string? InheritFrom
    {
        get;
    }

    public ImmutableArray<IDocumentationSegment> Summary
    {
        get;
    }

    public ImmutableDictionary<string, ImmutableArray<IDocumentationSegment>> Parameters
    {
        get;
    }

    public ImmutableDictionary<string, ImmutableArray<IDocumentationSegment>> TypeParameters
    {
        get;
    }

    public ImmutableDictionary<string, ImmutableArray<IDocumentationSegment>> Exceptions
    {
        get;
    }

    public ImmutableArray<IDocumentationSegment> Returns
    {
        get;
    }

    public ImmutableArray<IDocumentationSegment> Examples
    {
        get;
    }

    public ImmutableArray<IDocumentationSegment> Remarks
    {
        get;
    }

    public ImmutableArray<string> Seealso
    {
        get;
    }
}