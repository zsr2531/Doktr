using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class ValueTupleTypeSignatureTests
{
    [Fact]
    public void Int_And_String()
    {
        var signature = new ValueTupleTypeSignature
        {
            Parameters = new TypeSignatureCollection
            {
                new VanillaTypeSignature(new CodeReference("T:System.Int32")),
                new VanillaTypeSignature(new CodeReference("T:System.String"))
            }
        };
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("(int, string)", decompiled);
    }

    [Fact]
    public void Ulong_And_Bool_And_Test()
    {
        var signature = new ValueTupleTypeSignature
        {
            Parameters = new TypeSignatureCollection
            {
                new VanillaTypeSignature(new CodeReference("T:System.UInt64")),
                new VanillaTypeSignature(new CodeReference("T:System.Boolean")),
                new VanillaTypeSignature(new CodeReference("T:Test"))
            }
        };
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("(ulong, bool, Test)", decompiled);
    }

    [Fact]
    public void Nullable_Short_And_SByte_Array_Byte_Pointer()
    {
        var signature = new NullableValueTypeSignature(new ValueTupleTypeSignature
        {
            Parameters = new TypeSignatureCollection
            {
                new VanillaTypeSignature(new CodeReference("T:System.Int16")),
                new SzArrayTypeSignature(new VanillaTypeSignature(new CodeReference("T:System.SByte"))),
                new PointerTypeSignature(new VanillaTypeSignature(new CodeReference("T:System.Byte")))
            }
        });
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("(short, sbyte[], byte*)?", decompiled);
    }
}