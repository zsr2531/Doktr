using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class EventDocumentation : MemberDocumentation, IHasStatic, IHasVirtual
{
    public EventDocumentation(string name, MemberVisibility visibility, TypeSignature handlerType)
        : base(name, visibility)
    {
        HandlerType = handlerType;
    }

    public TypeSignature HandlerType { get; set; }
    public bool IsStatic { get; set; }
    public bool IsVirtual { get; set; }
    [MemberNotNullWhen(true, nameof(Overrides))]
    public bool IsOverride { get; set; }
    public CodeReference? Overrides { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitEvent(this);

    public override EventDocumentation Clone()
    {
        var clone = new EventDocumentation(Name, Visibility, HandlerType.Clone());
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is EventDocumentation otherEvent)
        {
            otherEvent.IsStatic = IsStatic;
            otherEvent.IsVirtual = IsVirtual;
            otherEvent.IsOverride = IsOverride;
            otherEvent.Overrides = Overrides;
            otherEvent.IsAbstract = IsAbstract;
            otherEvent.IsSealed = IsSealed;
        }

        base.CopyDocumentationTo(other);
    }
}