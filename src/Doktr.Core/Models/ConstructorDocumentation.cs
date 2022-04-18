using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public class ConstructorDocumentation : MemberDocumentation, IHasParameters, IHasExceptions
{
    public ConstructorDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public ParameterSegmentCollection Parameters { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();
}