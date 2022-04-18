using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class ExceptionSegment
{
    public ExceptionSegment(CodeReference exceptionType)
    {
        ExceptionType = exceptionType;
    }

    public CodeReference ExceptionType { get; set; }
    public DocumentationFragmentCollection Documentation { get; set; } = new();
}