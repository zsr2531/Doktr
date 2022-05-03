using System.Threading.Tasks;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Members;
using Doktr.Decompiler.Signatures;
using MediatR;
using NSubstitute;
using Xunit;

namespace Doktr.Decompiler.Tests.Members;

public class Attributes
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
    public void Event()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var eventDocumentation = new EventDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.EventHandler")));

        eventDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public event EventHandler Test", decompiled);
    }

    [Fact]
    public void Field()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var fieldDocumentation = new FieldDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.String")));

        fieldDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public string Test", decompiled);
    }

    [Fact]
    public void ReadOnly_Field()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var fieldDocumentation = new FieldDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.String")))
        {
            IsReadOnly = true
        };

        fieldDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public readonly string Test", decompiled);
    }

    [Fact]
    public void Constant_Field()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var fieldDocumentation = new FieldDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Int32")))
        {
            IsConstant = true,
            ConstantValue = 123
        };

        fieldDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public const int Test = 123", decompiled);
    }

    [Fact]
    public void Indexer()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var indexerDocumentation = new IndexerDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.String")))
        {
            Parameters = new ParameterSegmentCollection
            {
                new(new VanillaTypeSignature(new CodeReference("T:System.Int32")), "index")
            },
            Getter = new PropertyDocumentation.PropertyGetter(),
            Setter = new PropertyDocumentation.PropertySetter()
        };

        indexerDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public string this[int index] { get; set; }", decompiled);
    }

    [Fact]
    public void Get_Property()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var propertyDocumentation = new PropertyDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.String")))
        {
            Getter = new PropertyDocumentation.PropertyGetter()
        };

        propertyDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public string Test { get; }", decompiled);
    }

    [Fact]
    public void Get_Set_Property()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var propertyDocumentation = new PropertyDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.String")))
        {
            Getter = new PropertyDocumentation.PropertyGetter(),
            Setter = new PropertyDocumentation.PropertySetter()
        };

        propertyDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public string Test { get; set; }", decompiled);
    }

    [Fact]
    public void Get_Init_Property()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var propertyDocumentation = new PropertyDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.String")))
        {
            Getter = new PropertyDocumentation.PropertyGetter(),
            Setter = new PropertyDocumentation.PropertySetter
            {
                IsInit = true
            }
        };

        propertyDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public string Test { get; init; }", decompiled);
    }

    [Fact]
    public void Get_Property_With_Protected_Setter()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var propertyDocumentation = new PropertyDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.String")))
        {
            Getter = new PropertyDocumentation.PropertyGetter(),
            Setter = new PropertyDocumentation.PropertySetter
            {
                Visibility = MemberVisibility.Protected
            }
        };

        propertyDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public string Test { get; protected set; }", decompiled);
    }
}