using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public class ConstructorDocumentation : MemberDocumentation, IHasParameters, IHasExceptions
{
    public ConstructorDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public ParameterDocumentationCollection Parameters { get; set; } = new();
    public ExceptionDocumentationCollection Exceptions { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitConstructor(this);

    public override ConstructorDocumentation Clone()
    {
        var clone = new ConstructorDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is ConstructorDocumentation otherConstructor)
        {
            otherConstructor.Parameters = Parameters.Clone();
            otherConstructor.Exceptions = Exceptions.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}