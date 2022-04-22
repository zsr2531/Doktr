using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public class RecordDocumentation : CompositeTypeDocumentation, IHasBaseType, IHasParameters
{
    public RecordDocumentation(string name, MemberVisibility visibility)
        : base(name, visibility)
    {
    }

    public CodeReference? BaseType { get; set; }
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
            otherRecord.BaseType = BaseType;
            otherRecord.Parameters = Parameters.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}