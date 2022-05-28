using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class VanillaTypeSignatureTests
{
    [Fact]
    public void CustomType_Normal()
    {
        var signature = new VanillaTypeSignature(new CodeReference("T:My.Test"));
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test", decompiled);
    }

    [Fact]
    public void CustomType_Normal_WithNullable()
    {
        var signature = new VanillaTypeSignature(new CodeReference("T:My.Test"))
        {
            Nullability = NullabilityKind.Nullable
        };
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test", decompiled);
    }

    [Fact]
    public void CustomType_NotNullable()
    {
        var signature = new VanillaTypeSignature(new CodeReference("T:My.Test"));
        var decompiler = new NullableTypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test", decompiled);
    }

    [Fact]
    public void CustomType_Nullable()
    {
        var signature = new VanillaTypeSignature(new CodeReference("T:My.Test"))
        {
            Nullability = NullabilityKind.Nullable
        };
        var decompiler = new NullableTypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test?", decompiled);
    }

    [Fact]
    public void Primitive_Int32()
    {
        var signature = new VanillaTypeSignature(new CodeReference("T:System.Int32"));
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("int", decompiled);
    }

    [Fact]
    public void Primitive_String()
    {
        var signature = new VanillaTypeSignature(new CodeReference("T:System.String"));
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("string", decompiled);
    }
}