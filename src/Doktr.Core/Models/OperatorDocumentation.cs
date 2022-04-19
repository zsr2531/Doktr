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
    public OperatorDocumentation(string name, MemberVisibility visibility, TypeSignature returnType, OperatorKind kind)
        : base(name, visibility)
    {
        ReturnType = returnType;
        Kind = kind;
    }

    public OperatorKind Kind { get; set; }
    public bool IsStatic => true;
    public ParameterSegmentCollection Parameters { get; } = new();
    public TypeSignature ReturnType { get; set; }
    public DocumentationFragmentCollection Returns { get; } = new();
    public ExceptionSegmentCollection Exceptions { get; } = new();
}