using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public class ExceptionSegment
{
    public ExceptionSegment(CodeReference exceptionReference)
    {
        ExceptionReference = exceptionReference;
    }

    public CodeReference ExceptionReference { get; set; }
    public DocumentationFragmentCollection Documentation { get; set; } = new();
}