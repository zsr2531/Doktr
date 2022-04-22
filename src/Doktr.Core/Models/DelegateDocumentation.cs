using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class DelegateDocumentation : TypeDocumentation, IHasParameters, IHasReturns
{
    public DelegateDocumentation(string name, MemberVisibility visibility, TypeSignature returnType)
        : base(name, visibility)
    {
        ReturnType = returnType;
    }

    public ParameterSegmentCollection Parameters { get; set; } = new();
    public TypeSignature ReturnType { get; set; }
    public DocumentationFragmentCollection Returns { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitDelegate(this);

    public override DelegateDocumentation Clone()
    {
        var clone = new DelegateDocumentation(Name, Visibility, ReturnType.Clone());
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is DelegateDocumentation otherDelegate)
        {
            otherDelegate.Parameters = Parameters.Clone();
            otherDelegate.Returns = Returns.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}