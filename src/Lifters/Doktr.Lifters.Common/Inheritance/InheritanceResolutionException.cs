using Doktr.Core.Models;

namespace Doktr.Lifters.Common.Inheritance;

public class InheritanceResolutionException : Exception
{
    public InheritanceResolutionException(string message, CodeReference codeRef)
        : this(message, new[] { codeRef })
    {
    }

    public InheritanceResolutionException(string message, CodeReference[] affectedCodeReferences)
        : base(message)
    {
        AffectedCodeReferences = affectedCodeReferences;
    }

    public CodeReference[] AffectedCodeReferences { get; }
}