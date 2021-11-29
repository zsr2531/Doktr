using System.Collections.Immutable;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Models;

public interface IMemberDocumentation
{
    string Name
    {
        get;
    }
        
    ImmutableArray<IDocumentationSegment> Summary
    {
        get;
    }
        
    string? Syntax
    {
        get;
    }
        
    ImmutableArray<IDocumentationSegment> Examples
    {
        get;
    }
        
    ImmutableArray<IDocumentationSegment> Remarks
    {
        get;
    }
        
    ImmutableArray<IReference> SeeAlso
    {
        get;
    }
}