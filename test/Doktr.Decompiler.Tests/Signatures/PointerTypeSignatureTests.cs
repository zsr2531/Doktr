using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class PointerTypeSignatureTests
{
    [Fact]
    public void Void()
    {
        var signature = new PointerTypeSignature(new VanillaTypeSignature(new CodeReference("T:System.Void")));
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("void*", decompiled);
    }

    [Fact]
    public void Int_With_Multiple_Indirections()
    {
        var signature =
            new PointerTypeSignature(new PointerTypeSignature(
                new PointerTypeSignature(new VanillaTypeSignature(new CodeReference("T:System.Int32")))));
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("int***", decompiled);
    }
}