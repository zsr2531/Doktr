using System.Text;
using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;

namespace Doktr.Decompiler.Signatures;

public class TypeSignatureDecompilationStrategy : ITypeSignatureVisitor
{
    private static readonly Dictionary<CodeReference, string> Primitives = new()
    {
        [new CodeReference("T:System.Int8")] = "sbyte",
        [new CodeReference("T:System.Int16")] = "short",
        [new CodeReference("T:System.Int32")] = "int",
        [new CodeReference("T:System.Int64")] = "long",
        [new CodeReference("T:System.IntPtr")] = "nint",
        [new CodeReference("T:System.UInt8")] = "byte",
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

    public virtual void VisitVanilla(VanillaTypeSignature vanillaTypeSignature)
    {
        var codeReference = vanillaTypeSignature.Type;
        if (Primitives.TryGetValue(codeReference, out string? primitive))
        {
            Builder.Append(primitive);
            return;
        }

        var fullName = vanillaTypeSignature.Type.Name;
        var name = fullName.TrimUntilLastDot();
        var ticksRemoved = name.TrimTicks();

        Builder.Append(ticksRemoved);
    }

    public virtual void VisitGenericInstance(GenericInstanceTypeSignature genericInstanceTypeSignature)
    {
        genericInstanceTypeSignature.GenericType.AcceptVisitor(this);
        Builder.Append('<');

        var typeArgs = genericInstanceTypeSignature.TypeArguments;
        for (int i = 0; i < typeArgs.Count; i++)
        {
            var current = typeArgs[i];
            current.AcceptVisitor(this);

            if (i + 1 < typeArgs.Count)
                Builder.Append(", ");
        }

        Builder.Append('>');
    }

    public virtual void VisitGenericParameter(GenericParameterTypeSignature genericParameterTypeSignature)
    {
        Builder.Append(genericParameterTypeSignature.Name);
    }

    public virtual void VisitSzArray(SzArrayTypeSignature szArrayTypeSignature)
    {
        szArrayTypeSignature.ArrayType.AcceptVisitor(this);
        Builder.Append("[]");
    }

    public override string ToString() => Builder.ToString();
}