namespace Doktr.Core.Models.Constants;

public class StringConstant : Constant
{
    public StringConstant(string value)
    {
        Value = value;
    }

    public string Value { get; set; }

    public override void AcceptVisitor(IConstantVisitor visitor) => visitor.VisitString(this);

    public override StringConstant Clone() => new(Value);
}