using Doktr.Core.Models;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class SzArrayTypeSignatureTests
{
    [Fact]
    public void Int_Array()
    {
        var integer = new VanillaTypeSignature(new CodeReference("T:System.Int32"));
        var array = new SzArrayTypeSignature(integer);
        var decompiler = new TypeSignatureDecompilationStrategy();

        array.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("int[]", decompiled);
    }

    [Fact]
    public void Multidimensional_Array()
    {
        var integer = new VanillaTypeSignature(new CodeReference("T:System.Int32"));
        var array1 = new SzArrayTypeSignature(integer);
        var array2 = new SzArrayTypeSignature(array1)
        {
            Nullability = NullabilityKind.Nullable
        };
        var array3 = new SzArrayTypeSignature(array2);
        var decompiler = new NullableTypeSignatureDecompilationStrategy();

        array3.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("int[][]?[]", decompiled);
    }
}