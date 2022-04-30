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

    public void VisitConstructor(ConstructorDocumentation constructorDocumentation)
    {
        WriteVisibility(constructorDocumentation);

        _sb.Append(constructorDocumentation.Name);

        WriteParameters(constructorDocumentation);
    }

    public void VisitFinalizer(FinalizerDocumentation finalizerDocumentation)
    {
        _sb.Append(finalizerDocumentation.Name);
        _sb.Append("()");
    }

    public void VisitMethod(MethodDocumentation methodDocumentation)
    {
        WriteVisibility(methodDocumentation);
        WriteStatic(methodDocumentation);
        WriteVirtual(methodDocumentation);
        WriteReadOnly(methodDocumentation);
        WriteReturnType(methodDocumentation);

        _sb.Append(' ');
        _sb.Append(methodDocumentation.Name);

        WriteTypeParameters(methodDocumentation);
        WriteParameters(methodDocumentation);
        WriteTypeParameterConstraints(methodDocumentation);
    }

    public void VisitOperator(OperatorDocumentation operatorDocumentation)
    {
        WriteVisibility(operatorDocumentation);
        WriteStatic(operatorDocumentation);

        WriteReturnType(operatorDocumentation);

        _sb.Append(" operator ");
        _sb.Append(OperatorSymbols[operatorDocumentation.Symbol]);

        WriteParameters(operatorDocumentation);
    }

    public void VisitConversionOperator(ConversionOperatorDocumentation conversionOperatorDocumentation)
    {
        WriteVisibility(conversionOperatorDocumentation);
        WriteStatic(conversionOperatorDocumentation);

        _sb.Append(conversionOperatorDocumentation.Kind switch
        {
            ConversionKind.Implicit => "implicit ",
            ConversionKind.Explicit => "explicit ",
            _ => throw new ArgumentOutOfRangeException(nameof(conversionOperatorDocumentation))
        });

        _sb.Append("operator ");

        WriteReturnType(conversionOperatorDocumentation);
        WriteParameters(conversionOperatorDocumentation);
    }

    private void WriteReturnType(IHasReturns member)
    {
        var returnType = member.ReturnType;
        string decompiled = DecompileTypeSignature(returnType);

        _sb.Append(decompiled);
    }
}