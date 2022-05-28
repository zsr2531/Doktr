using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public class ExceptionDocumentation : ICloneable
{
    public ExceptionDocumentation(CodeReference exceptionType)
    {
        ExceptionType = exceptionType;
    }

    public CodeReference ExceptionType { get; set; }
    public DocumentationFragmentCollection ThrownWhen { get; set; } = new();

    public ExceptionDocumentation Clone() => new(ExceptionType)
    {
        ThrownWhen = ThrownWhen.Clone()
    };
    
    object ICloneable.Clone() => Clone();
}