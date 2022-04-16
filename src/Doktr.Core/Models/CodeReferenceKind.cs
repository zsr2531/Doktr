namespace Doktr.Core.Models;

public enum CodeReferenceKind
{
    Error,
    Type,
    Method,
    Property,
    Field,
    Event,
    Namespace // This is unused for .NET, but it's here for completeness.
}