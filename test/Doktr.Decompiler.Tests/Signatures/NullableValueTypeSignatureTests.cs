using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class NullableValueTypeSignatureTests
{
    [Fact]
    public void Int32()
    {
        var signature = new NullableValueTypeSignature(new VanillaTypeSignature(new CodeReference("T:System.Int32")));
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("int?", decompiled);
    }

    [Fact]
    public void CustomType()
    {
        var signature = new NullableValueTypeSignature(new VanillaTypeSignature(new CodeReference("T:MyStruct")));
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("MyStruct?", decompiled);
    }
}