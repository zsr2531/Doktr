using Doktr.Core.Models;

namespace Doktr.Decompiler.Members;

public partial class MemberDecompiler
{
    private static readonly Dictionary<OperatorKind, string> OperatorSymbols = new()
    {
        [OperatorKind.UnaryPlus] = "+",
        [OperatorKind.UnaryMinus] = "-",
        [OperatorKind.UnaryBang] = "!",
        [OperatorKind.UnaryTilde] = "~",
        [OperatorKind.UnaryPlusPlus] = "++",
        [OperatorKind.UnaryMinusMinus] = "--",
        [OperatorKind.UnaryTrue] = "true",
        [OperatorKind.UnaryFalse] = "false",

        [OperatorKind.BinaryPlus] = "+",
        [OperatorKind.BinaryMinus] = "-",
        [OperatorKind.BinaryStar] = "*",
        [OperatorKind.BinarySlash] = "/",
        [OperatorKind.BinaryPercent] = "%",
        [OperatorKind.BinaryAmpersand] = "&",
        [OperatorKind.BinaryPipe] = "|",
        [OperatorKind.BinaryCaret] = "^",
        [OperatorKind.BinaryLeftDoubleAngle] = "<<",
        [OperatorKind.BinaryRightDoubleAngle] = ">>",

        [OperatorKind.BinaryDoubleEqual] = "==",
        [OperatorKind.BinaryBangEqual] = "!=",
        [OperatorKind.BinaryLeftAngle] = "<",
        [OperatorKind.BinaryRightAngle] = ">",
        [OperatorKind.BinaryLeftAngleEqual] = "<=",
        [OperatorKind.BinaryRightAngleEqual] = ">="
    };

    public void VisitConstructor(ConstructorDocumentation documentation)
    {
        WriteVisibility(documentation);

        _sb.Append(documentation.Name);

        WriteParameters(documentation);
    }

    public void VisitFinalizer(FinalizerDocumentation documentation)
    {
        _sb.Append(documentation.Name);
        _sb.Append("()");
    }

    public void VisitMethod(MethodDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteStatic(documentation);
        WriteVirtual(documentation);
        WriteReadOnly(documentation);
        WriteReturnType(documentation);

        _sb.Append(' ');
        _sb.Append(documentation.Name);

        WriteTypeParameters(documentation);
        WriteParameters(documentation);
        WriteTypeParameterConstraints(documentation);
    }

    public void VisitOperator(OperatorDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteStatic(documentation);

        WriteReturnType(documentation);

        _sb.Append(" operator ");
        _sb.Append(OperatorSymbols[documentation.Symbol]);

        WriteParameters(documentation);
    }

    public void VisitConversionOperator(ConversionOperatorDocumentation documentation)
    {
        WriteVisibility(documentation);
        WriteStatic(documentation);

        _sb.Append(documentation.Kind switch
        {
            ConversionKind.Implicit => "implicit ",
            ConversionKind.Explicit => "explicit ",
            _ => throw new ArgumentOutOfRangeException(nameof(documentation))
        });

        _sb.Append("operator ");

        WriteReturnType(documentation);
        WriteParameters(documentation);
    }

    private void WriteReturnType(IHasReturns member) => WriteTypeSignature(member.ReturnType);
}