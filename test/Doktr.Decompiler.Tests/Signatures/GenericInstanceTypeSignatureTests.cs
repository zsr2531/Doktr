using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class GenericInstanceTypeSignatureTests
{
    [Fact]
    public void CustomType_With_GenericParameter_Normal()
    {
        var genericTypeSignature = new VanillaTypeSignature(new CodeReference("T:My.Test"));
        var genericParameter = new GenericParameterTypeSignature("T");
        var signature = new GenericInstanceTypeSignature(genericTypeSignature);
        signature.TypeParameters.Add(genericParameter);
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test<T>", decompiled);
    }

    [Fact]
    public void CustomType_With_Multiple_GenericParameters_Normal()
    {
        var genericTypeSignature = new VanillaTypeSignature(new CodeReference("T:My.Test"));
        var genericParameter = new GenericParameterTypeSignature("T");
        var genericParameter2 = new GenericParameterTypeSignature("U");
        var genericParameter3 = new GenericParameterTypeSignature("V");
        var signature = new GenericInstanceTypeSignature(genericTypeSignature);
        signature.TypeParameters.Add(genericParameter);
        signature.TypeParameters.Add(genericParameter2);
        signature.TypeParameters.Add(genericParameter3);
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test<T, U, V>", decompiled);
    }

    [Fact]
    public void CustomType_With_Nullable_GenericParameter_Normal()
    {
        var genericTypeSignature = new VanillaTypeSignature(new CodeReference("T:My.Test"));
        var genericParameter = new GenericParameterTypeSignature("T")
        {
            Nullability = NullabilityKind.Nullable
        };
        var signature = new GenericInstanceTypeSignature(genericTypeSignature);
        signature.TypeParameters.Add(genericParameter);
        var decompiler = new TypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test<T>", decompiled);
    }

    [Fact]
    public void CustomType_With_Nullable_GenericParameter_Nullable()
    {
        var genericTypeSignature = new VanillaTypeSignature(new CodeReference("T:My.Test"));
        var genericParameter = new GenericParameterTypeSignature("T")
        {
            Nullability = NullabilityKind.Nullable
        };
        var signature = new GenericInstanceTypeSignature(genericTypeSignature);
        signature.TypeParameters.Add(genericParameter);
        var decompiler = new NullableTypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test<T?>", decompiled);
    }

    [Fact]
    public void Custom_Type_With_Normal_And_Nullable_GenericParameters_Nullable()
    {
        var genericTypeSignature = new VanillaTypeSignature(new CodeReference("T:My.Test"));
        var genericParameter = new GenericParameterTypeSignature("T")
        {
            Nullability = NullabilityKind.Nullable
        };
        var genericParameter2 = new GenericParameterTypeSignature("U")
        {
            Nullability = NullabilityKind.Nullable
        };
        var signature = new GenericInstanceTypeSignature(genericTypeSignature);
        signature.TypeParameters.Add(genericParameter);
        signature.TypeParameters.Add(genericParameter2);
        var decompiler = new NullableTypeSignatureDecompilationStrategy();
        signature.AcceptVisitor(decompiler);

        string decompiled = decompiler.ToString();
        Assert.Equal("Test<T?, U?>", decompiled);
    }
}