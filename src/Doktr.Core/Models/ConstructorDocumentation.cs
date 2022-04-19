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
        var clone = new ConstructorDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is not ConstructorDocumentation otherConstructor)
            throw new InvalidOperationException("Cannot copy documentation to a non-constructor member.");
        
        otherConstructor.Parameters = Parameters.Clone();
        otherConstructor.Exceptions = Exceptions.Clone();
        base.CopyDocumentationTo(other);
    }
}