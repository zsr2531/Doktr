using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models;

public enum OperatorKind
{
    UnaryPlus,
    UnaryMinus,
    UnaryBang,
    UnaryTilde,
    UnaryPlusPlus,
    UnaryMinusMinus,
    UnaryTrue,
    UnaryFalse,

    BinaryPlus,
    BinaryMinus,
    BinaryStar,
    BinarySlash,
    BinaryPercent,
    BinaryAmpersand,
    BinaryPipe,
    BinaryCaret,
    BinaryLeftDoubleAngle,
    BinaryRightDoubleAngle,

    BinaryDoubleEqual,
    BinaryBangEqual,
    BinaryLeftAngle,
    BinaryRightAngle,
    BinaryLeftAngleEqual,
    BinaryRightAngleEqual
}

public class OperatorDocumentation : MemberDocumentation, IHasStatic, IHasParameters, IHasReturns, IHasExceptions
{
    public OperatorDocumentation(
        string name,
        MemberVisibility visibility,
        TypeSignature returnType,
        OperatorKind symbol)
        : base(name, visibility)
    {
        ReturnType = returnType;
        Symbol = symbol;
    }

    public OperatorKind Symbol { get; set; }
    public bool IsStatic => true;
    public ParameterDocumentationCollection Parameters { get; set; } = new();
    public TypeSignature ReturnType { get; set; }
    public DocumentationFragmentCollection Returns { get; set; } = new();
    public ExceptionSegmentCollection Exceptions { get; set; } = new();

    public override void AcceptVisitor(IDocumentationMemberVisitor visitor) => visitor.VisitOperator(this);

    public override OperatorDocumentation Clone()
    {
        var clone = new OperatorDocumentation(Name, Visibility, ReturnType.Clone(), Symbol);
        CopyDocumentationTo(clone);
        
        return clone;
    }

    protected override void CopyDocumentationTo(MemberDocumentation other)
    {
        if (other is OperatorDocumentation otherOp)
        {
            otherOp.Parameters = Parameters.Clone();
            otherOp.Returns = Returns.Clone();
            otherOp.Exceptions = Exceptions.Clone();
        }

        base.CopyDocumentationTo(other);
    }
}