namespace Doktr.Core.Models.Constants;

public class DefaultConstant : Constant
{
    public override void AcceptVisitor(IConstantVisitor visitor) => visitor.VisitDefault(this);

    public override DefaultConstant Clone() => new();
}