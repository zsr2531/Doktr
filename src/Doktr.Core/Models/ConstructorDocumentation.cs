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

    public override ConstructorDocumentation Clone()
    {
        return new ConstructorDocumentation(Name, Visibility)
        {
            Summary = Summary.Clone(),
            Parameters = Parameters.Clone(),
            Exceptions = Exceptions.Clone(),
            Examples = Examples.Clone(),
            Remarks = Remarks.Clone(),
            AppliesTo = AppliesTo.Clone()
        };
    }
}