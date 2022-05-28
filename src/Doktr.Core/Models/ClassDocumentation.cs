using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class ClassDocumentation : CompositeTypeDocumentation, IHasAbstract, IHasBaseType, IHasFinalizer
{
    public ClassDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }


    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public TypeSignature? BaseType { get; set; }
    public FinalizerDocumentation? Finalizer { get; set; }

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitClass(this);

    public override ClassDocumentation Clone()
    {
        var clone = new ClassDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is ClassDocumentation otherClass)
        {
            otherClass.IsAbstract = IsAbstract;
            otherClass.IsSealed = IsSealed;
            otherClass.BaseType = BaseType;
            otherClass.Finalizer = Finalizer?.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}