using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class NestedTypeSignatureTests
{
    [Fact]
    public void Two_Levels()
    {
        var parent = new VanillaTypeSignature(new CodeReference("T:Test"));
        var child = new VanillaTypeSignature(new CodeReference("T:Test.Child"));
        var signature = new NestedTypeSignature(parent, child);
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("Test.Child", decompiled);
    }

    [Fact]
    public void Three_Levels()
    {
        var parent = new VanillaTypeSignature(new CodeReference("T:Test"));
        var child = new VanillaTypeSignature(new CodeReference("T:Test.Child"));
        var grandChild = new VanillaTypeSignature(new CodeReference("T:Test.Child.GrandChild"));
        var signature = new NestedTypeSignature(parent, new NestedTypeSignature(child, grandChild));
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("Test.Child.GrandChild", decompiled);
    }
}