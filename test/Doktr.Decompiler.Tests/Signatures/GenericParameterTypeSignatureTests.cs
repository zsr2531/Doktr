using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class GenericParameterTypeSignatureTests
{
    [Fact]
    public void Normal()
    {
        var signature = new GenericParameterTypeSignature("T");
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("T", decompiled);
    }

    [Fact]
    public void Normal_WithNullable()
    {
        var signature = new GenericParameterTypeSignature("T")
        {
            Nullability = NullabilityKind.Nullable
        };
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("T", decompiled);
    }

    [Fact]
    public void NotNullable()
    {
        var signature = new GenericParameterTypeSignature("T");
        var decompiler = new NullableTypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("T", decompiled);
    }

    [Fact]
    public void Nullable()
    {
        var signature = new GenericParameterTypeSignature("T")
        {
            Nullability = NullabilityKind.Nullable
        };
        var decompiler = new NullableTypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("T?", decompiled);
    }
}