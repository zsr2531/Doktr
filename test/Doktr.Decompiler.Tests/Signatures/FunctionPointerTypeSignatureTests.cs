using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Signatures;
using Xunit;

namespace Doktr.Decompiler.Tests.Signatures;

public class FunctionPointerTypeSignatureTests
{
    [Fact]
    public void Managed()
    {
        var signature = new FunctionPointerTypeSignature
        {
            CallingConvention = CallingConventions.Managed,
            Parameters = new TypeSignatureCollection
            {
                new VanillaTypeSignature(new CodeReference("T:System.Int32")),
                new VanillaTypeSignature(new CodeReference("T:System.Int32"))
            }
        };
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("delegate*<int, int>", decompiled);
    }

    [Theory]
    [InlineData(CallingConventions.Cdecl, "Cdecl")]
    [InlineData(CallingConventions.Fastcall, "Fastcall")]
    [InlineData(CallingConventions.Stdcall, "Stdcall")]
    [InlineData(CallingConventions.Thiscall, "Thiscall")]
    [InlineData(CallingConventions.MemberFunction, "MemberFunction")]
    [InlineData(CallingConventions.SuppressGCTransition, "SuppressGCTransition")]
    public void Unmanaged(CallingConventions conv, string raw)
    {
        var signature = new FunctionPointerTypeSignature
        {
            CallingConvention = conv,
            Parameters = new TypeSignatureCollection
            {
                new VanillaTypeSignature(new CodeReference("T:System.UInt64")),
                new VanillaTypeSignature(new CodeReference("T:System.Void")),
            }
        };
        var decompiler = new TypeSignatureDecompilationStrategy();

        signature.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal($"delegate*[{raw}]<ulong, void>", decompiled);
    }
}