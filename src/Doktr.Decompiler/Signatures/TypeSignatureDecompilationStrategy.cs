using System.Text;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

public class TypeSignatureDecompilationStrategy : ITypeSignatureVisitor
{
    private static readonly Dictionary<CodeReference, string> Primitives = new()
    {
        [new CodeReference("T:System.Void")] = "void",
        [new CodeReference("T:System.SByte")] = "sbyte",
        [new CodeReference("T:System.Int16")] = "short",
        [new CodeReference("T:System.Int32")] = "int",
        [new CodeReference("T:System.Int64")] = "long",
        [new CodeReference("T:System.IntPtr")] = "nint",
        [new CodeReference("T:System.Byte")] = "byte",
        [new CodeReference("T:System.UInt16")] = "ushort",
        [new CodeReference("T:System.UInt32")] = "uint",
        [new CodeReference("T:System.UInt64")] = "ulong",
        [new CodeReference("T:System.UIntPtr")] = "nuint",
        [new CodeReference("T:System.Single")] = "float",
        [new CodeReference("T:System.Double")] = "double",
        [new CodeReference("T:System.Boolean")] = "bool",
        [new CodeReference("T:System.Char")] = "char",
        [new CodeReference("T:System.Void")] = "void",
        [new CodeReference("T:System.String")] = "string",
        [new CodeReference("T:System.Object")] = "object",
    };

    protected readonly StringBuilder Builder = new();

    public virtual void VisitVanilla(VanillaTypeSignature signature)
    {
        var codeReference = signature.Type;
        if (Primitives.TryGetValue(codeReference, out string? primitive))
        {
            Builder.Append(primitive);
            return;
        }

        var fullName = signature.Type.Name;
        var name = fullName.TrimUntilLastDot();
        var ticksRemoved = name.TrimTicks();

        Builder.Append(ticksRemoved);
    }

    public virtual void VisitGenericInstance(GenericInstanceTypeSignature signature)
    {
        signature.GenericType.AcceptVisitor(this);
        Builder.Append('<');

        var typeParams = signature.TypeParameters;
        WriteTypeSignatures(typeParams);

        Builder.Append('>');
    }

    public virtual void VisitGenericParameter(GenericParameterTypeSignature signature)
    {
        Builder.Append(signature.Name);
    }

    public virtual void VisitSzArray(SzArrayTypeSignature signature)
    {
        signature.ArrayType.AcceptVisitor(this);
        Builder.Append("[]");
    }

    public void VisitNullableValue(NullableValueTypeSignature signature)
    {
        signature.ValueType.AcceptVisitor(this);
        Builder.Append('?');
    }

    public void VisitValueTuple(ValueTupleTypeSignature signature)
    {
        Builder.Append('(');

        var parameters = signature.Parameters;
        WriteTypeSignatures(parameters);

        Builder.Append(')');
    }

    public void VisitPointer(PointerTypeSignature signature)
    {
        signature.PointedToType.AcceptVisitor(this);
        Builder.Append('*');
    }

    public virtual void VisitJaggedArray(JaggedArrayTypeSignature signature)
    {
        signature.ArrayType.AcceptVisitor(this);
        Builder.Append('[');

        for (int i = 1; i < signature.Dimensions; i++)
            Builder.Append(',');

        Builder.Append(']');
    }

    public void VisitFunctionPointer(FunctionPointerTypeSignature signature)
    {
        Builder.Append("delegate*");
        WriteCallingConvention(signature.CallingConvention);

        Builder.Append('<');

        var parameters = signature.Parameters;
        WriteTypeSignatures(parameters);

        Builder.Append('>');
    }

    private void WriteCallingConvention(CallingConventions callingConvention)
    {
        if (callingConvention == CallingConventions.Managed)
            return;

        Builder.Append('[');

        Builder.Append(callingConvention switch
        {
            CallingConventions.Cdecl => "Cdecl",
            CallingConventions.Fastcall => "Fastcall",
            CallingConventions.Stdcall => "Stdcall",
            CallingConventions.Thiscall => "Thiscall",
            CallingConventions.MemberFunction => "MemberFunction",
            CallingConventions.SuppressGCTransition => "SuppressGCTransition",
            _ => throw new ArgumentOutOfRangeException(nameof(callingConvention))
        });

        Builder.Append(']');
    }

    private void WriteTypeSignatures(TypeSignatureCollection signatures)
    {
        for (int i = 0; i < signatures.Count; i++)
        {
            var current = signatures[i];
            current.AcceptVisitor(this);

            if (i + 1 < signatures.Count)
                Builder.Append(", ");
        }
    }

    public override string ToString() => Builder.ToString();
}