namespace Doktr.Core.Models.Constants;

public class NullConstant : Constant
{
    public override void AcceptVisitor(IConstantVisitor visitor) => visitor.VisitNull(this);

    public override NullConstant Clone() => new();
}