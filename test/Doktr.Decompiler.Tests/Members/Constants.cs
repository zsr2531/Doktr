using System.Threading.Tasks;
using Doktr.Core;
using Doktr.Core.Models.Constants;
using Doktr.Decompiler.Members;
using Doktr.Decompiler.Signatures;
using MediatR;
using NSubstitute;
using Xunit;

namespace Doktr.Decompiler.Tests.Members;

public class Constants
{
    private static IMediator CreateMockMediator()
    {
        var mediator = Substitute.For<IMediator>();
        mediator.Send(Arg.Any<DecompileTypeSignature>())
                .Returns(info =>
                {
                    var request = info.Arg<DecompileTypeSignature>();
                    var decompiler = new NullableTypeSignatureDecompilationStrategy();
                    request.Signature.AcceptVisitor(decompiler);

                    return Task.FromResult(decompiler.ToString());
                });

        return mediator;
    }

    [Fact]
    public void Null()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var constant = new NullConstant();

        constant.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("null", decompiled);
    }

    [Fact]
    public void Default()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var constant = new DefaultConstant();

        constant.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("default", decompiled);
    }

    [Fact]
    public void Integer()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var constant = new ObjectConstant(0);

        constant.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("0", decompiled);
    }

    [Fact]
    public void String()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var constant = new StringConstant("Hello World");

        constant.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("\"Hello World\"", decompiled);
    }

    [Fact]
    public void String_With_Control_Characters()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var constant = new StringConstant("Hello\r\nWorld");

        constant.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("\"Hello\\r\\nWorld\"", decompiled);
    }
}