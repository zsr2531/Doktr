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
    public bool IsOverride { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }

    public override EventDocumentation Clone()
    {
        var clone = new EventDocumentation(Name, Visibility, HandlerType)
        {
            IsStatic = IsStatic,
            IsVirtual = IsVirtual,
            IsOverride = IsOverride,
            IsAbstract = IsAbstract,
            IsSealed = IsSealed,
        };
        
        CopyDocumentationTo(clone);
        return clone;
    }
}