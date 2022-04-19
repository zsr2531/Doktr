using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class ExceptionSegment : ICloneable
{
    public ExceptionSegment(CodeReference exceptionType)
    {
        ExceptionType = exceptionType;
    }

    public CodeReference ExceptionType { get; set; }
    public DocumentationFragmentCollection Documentation { get; set; } = new();

    public ExceptionSegment Clone() => new(ExceptionType)
    {
        Documentation = Documentation.Clone()
    };
    
    object ICloneable.Clone() => Clone();
}