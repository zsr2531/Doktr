using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public class RecordDocumentation : CompositeTypeDocumentation, IHasAbstract, IHasBaseType, IHasParameters
{
    public RecordDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public TypeSignature? BaseType { get; set; }
    public ParameterSegmentCollection Parameters { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitRecord(this);

    public override RecordDocumentation Clone()
    {
        var clone = new RecordDocumentation(Name, Visibility);
        CopyDocumentationTo(clone);

        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is RecordDocumentation otherRecord)
        {
            otherRecord.IsAbstract = IsAbstract;
            otherRecord.IsSealed = IsSealed;
            otherRecord.BaseType = BaseType;
            otherRecord.Parameters = Parameters.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}