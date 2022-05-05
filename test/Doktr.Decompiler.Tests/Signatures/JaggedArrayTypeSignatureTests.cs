using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class JaggedArrayTypeSignatureTests
{
    [Fact]
    public void Two_Dimensions()
    {
        var signature = new JaggedArrayTypeSignature(2, new VanillaTypeSignature(new CodeReference("T:System.Int32")));
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("int[,]", decompiled);
    }

    [Fact]
    public void Five_Dimensions()
    {
        var signature = new JaggedArrayTypeSignature(5, new VanillaTypeSignature(new CodeReference("T:Test")));
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("Test[,,,,]", decompiled);
    }
}