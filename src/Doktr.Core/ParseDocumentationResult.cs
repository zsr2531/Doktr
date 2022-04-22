using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models;

namespace Doktr.Core;

public readonly struct ParseDocumentationResult
{
    public ParseDocumentationResult(MemberDocumentation documentation)
    {
        IsSuccess = true;
        Documentation = documentation;
        Error = null;
    }

    public ParseDocumentationResult(Exception error)
    {
        IsSuccess = false;
        Documentation = null;
        Error = error;
    }

    [MemberNotNullWhen(true, nameof(Documentation))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    public MemberDocumentation? Documentation { get; }
    public Exception? Error { get; }
}