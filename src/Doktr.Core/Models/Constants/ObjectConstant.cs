namespace Doktr.Core.Models.Constants;

public class ObjectConstant : Constant
{
    public ObjectConstant(object value)
    {
        Value = value;
    }

    public object Value { get; set; }

    public override void AcceptVisitor(IConstantVisitor visitor) => visitor.VisitObject(this);

    public override ObjectConstant Clone() => new(Value);
}