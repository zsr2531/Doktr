using System.Threading.Tasks;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Constants;
using Doktr.Core.Models.Constraints;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Members;
using Doktr.Decompiler.Signatures;
using MediatR;
using NSubstitute;
using Xunit;

namespace Doktr.Decompiler.Tests.Members;

public class Methods
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
    public void Method()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")));

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public void Test()", decompiled);
    }

    [Fact]
    public void Method_With_TypeParameters_And_Parameters()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation =
            new MethodDocumentation("Test", MemberVisibility.Public, new GenericParameterTypeSignature("T"))
            {
                TypeParameters = new TypeParameterDocumentationCollection
                {
                    new("T")
                    {
                        Constraints = new TypeParameterConstraintCollection
                        {
                            new ValueTypeParameterConstraint { IsUnmanaged = true }
                        }
                    },
                    new("U")
                    {
                        Constraints = new TypeParameterConstraintCollection
                        {
                            new InterfaceTypeParameterConstraint(new VanillaTypeSignature(
                                new CodeReference("T:System.ICloneable")))
                        }
                    },
                    new("V")
                    {
                        Constraints = new TypeParameterConstraintCollection
                        {
                            new ReferenceTypeParameterConstraint
                            {
                                Nullability = NullabilityKind.Nullable
                            }
                        }
                    }
                },
                Parameters = new ParameterDocumentationCollection
                {
                    new(new GenericParameterTypeSignature("U"), "first"),
                    new(new GenericInstanceTypeSignature(new VanillaTypeSignature(
                        new CodeReference("T:System.IEquatable")))
                    {
                        TypeArguments = new TypeSignatureCollection
                        {
                            new GenericParameterTypeSignature("V")
                        }
                    }, "second")
                }
            };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal(
            "public T Test<T, U, V>(U first, IEquatable<V> second)\n    where T : unmanaged\n    where U : ICloneable\n    where V : class?",
            decompiled);
    }

    [Fact]
    public void Method_With_Optional_Parameters()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")))
        {
            Parameters = new ParameterDocumentationCollection
            {
                new(new VanillaTypeSignature(new CodeReference("T:System.Int32")), "first"),
                new(new VanillaTypeSignature(new CodeReference("T:System.Int32")), "second")
                {
                    Modifiers = ParameterModifierFlags.Optional,
                    DefaultValue = new ObjectConstant(123)
                },
            }
        };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal(
            "public void Test(int first, int second = 123)",
            decompiled);
    }

    [Fact]
    public void Abstract_Method()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")))
        {
            IsAbstract = true
        };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public abstract void Test()", decompiled);
    }

    [Fact]
    public void Virtual_Method()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")))
        {
            IsVirtual = true
        };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public virtual void Test()", decompiled);
    }

    [Fact]
    public void Sealed_Method()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")))
        {
            IsSealed = true
        };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public sealed void Test()", decompiled);
    }

    [Fact]
    public void Static_Method()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")))
        {
            IsStatic = true
        };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public static void Test()", decompiled);
    }

    [Fact]
    public void Sealed_Override_Method()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")))
        {
            IsSealed = true,
            IsOverride = true
        };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public sealed override void Test()", decompiled);
    }

    [Fact]
    public void ReadOnly_Method()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var methodDocumentation = new MethodDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Void")))
        {
            IsReadOnly = true
        };

        methodDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public readonly void Test()", decompiled);
    }

    [Fact]
    public void Constructor()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var constructorDocumentation = new ConstructorDocumentation("Test", MemberVisibility.Public);

        constructorDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public Test()", decompiled);
    }

    [Fact]
    public void Finalizer()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var finalizerDocumentation = new FinalizerDocumentation("~Test");

        finalizerDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("~Test()", decompiled);
    }

    [Fact]
    public void UnaryOperator()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var operatorDocumentation = new OperatorDocumentation("op_True", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Boolean")), OperatorKind.UnaryTrue)
        {
            Parameters = new ParameterDocumentationCollection
            {
                new(new VanillaTypeSignature(new CodeReference("T:System.Int32")), "value")
            }
        };

        operatorDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public static bool operator true(int value)", decompiled);
    }

    [Fact]
    public void BinaryOperator()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var operatorDocumentation = new OperatorDocumentation("op_Addition", MemberVisibility.Public,
            new VanillaTypeSignature(new CodeReference("T:System.Int32")), OperatorKind.UnaryPlus)
        {
            Parameters = new ParameterDocumentationCollection
            {
                new(new VanillaTypeSignature(new CodeReference("T:System.Int32")), "left"),
                new(new VanillaTypeSignature(new CodeReference("T:System.Int32")), "right")
            }
        };

        operatorDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public static int operator +(int left, int right)", decompiled);
    }

    [Fact]
    public void ExplicitConversionOperator()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var conversionOperatorDocumentation = new ConversionOperatorDocumentation("op_Explicit",
            MemberVisibility.Public, new VanillaTypeSignature(new CodeReference("T:System.Int32")),
            ConversionKind.Explicit)
        {
            Parameters = new ParameterDocumentationCollection
            {
                new(new VanillaTypeSignature(new CodeReference("T:System.Boolean")), "value")
            }
        };

        conversionOperatorDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public static explicit operator int(bool value)", decompiled);
    }

    [Fact]
    public void ImplicitConversionOperator()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var conversionOperatorDocumentation = new ConversionOperatorDocumentation("op_Implicit",
            MemberVisibility.Public, new VanillaTypeSignature(new CodeReference("T:System.Boolean")),
            ConversionKind.Implicit)
        {
            Parameters = new ParameterDocumentationCollection
            {
                new(new VanillaTypeSignature(new CodeReference("T:System.Int32")), "value")
            }
        };

        conversionOperatorDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public static implicit operator bool(int value)", decompiled);
    }
}