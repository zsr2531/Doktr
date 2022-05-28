using System.Threading.Tasks;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Constraints;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Members;
using Doktr.Decompiler.Signatures;
using MediatR;
using NSubstitute;
using Xunit;

namespace Doktr.Decompiler.Tests.Members;

public class Constraints
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
    public void ValueType()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var valueType = new ValueTypeParameterConstraint();

        valueType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("struct", decompiled);
    }

    [Fact]
    public void ValueType_Unmanaged()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var valueType = new ValueTypeParameterConstraint
        {
            IsUnmanaged = true
        };

        valueType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("unmanaged", decompiled);
    }

    [Fact]
    public void ReferenceType()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var referenceType = new ReferenceTypeParameterConstraint();

        referenceType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("class", decompiled);
    }

    [Fact]
    public void ReferenceType_Nullable()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var referenceType = new ReferenceTypeParameterConstraint
        {
            Nullability = NullabilityKind.Nullable
        };

        referenceType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("class?", decompiled);
    }

    [Fact]
    public void ReferenceType_CustomType()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var referenceType = new ReferenceTypeParameterConstraint
        {
            BaseType = new VanillaTypeSignature(new CodeReference("T:Test"))
        };

        referenceType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("Test", decompiled);
    }

    [Fact]
    public void ReferenceType_CustomType_Nullable()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var referenceType = new ReferenceTypeParameterConstraint
        {
            BaseType = new VanillaTypeSignature(new CodeReference("T:Test")),
            Nullability = NullabilityKind.Nullable
        };

        referenceType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("Test?", decompiled);
    }

    [Fact]
    public void Interface()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var interfaceType =
            new InterfaceTypeParameterConstraint(new VanillaTypeSignature(new CodeReference("T:ITest")));

        interfaceType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("ITest", decompiled);
    }

    [Fact]
    public void Interface_Nullable()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var interfaceType = new InterfaceTypeParameterConstraint(
            new VanillaTypeSignature(new CodeReference("T:ITest"))
            {
                Nullability = NullabilityKind.Nullable
            });

        interfaceType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("ITest?", decompiled);
    }

    [Fact]
    public void NotNull()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var notNullType = new NotNullTypeKindTypeParameterConstraint();

        notNullType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("notnull", decompiled);
    }

    [Fact]
    public void Default()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var defaultType = new DefaultTypeKindTypeParameterConstraint();

        defaultType.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("default", decompiled);
    }

    [Fact]
    public void Constructor()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var constructor = new ConstructorTypeParameterConstraint();

        constructor.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("new()", decompiled);
    }
}