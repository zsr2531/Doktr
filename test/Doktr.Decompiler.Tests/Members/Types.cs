using System.Threading.Tasks;
using Doktr.Core;
using Doktr.Core.Models;
using Doktr.Core.Models.Collections;
using Doktr.Core.Models.Segments;
using Doktr.Core.Models.Signatures;
using Doktr.Decompiler.Members;
using Doktr.Decompiler.Signatures;
using MediatR;
using NSubstitute;
using Xunit;

namespace Doktr.Decompiler.Tests.Members;

public class Types
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
    public void Class()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public);

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test", decompiled);
    }

    [Fact]
    public void Class_With_BaseType()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            BaseType = new VanillaTypeSignature("Base", new CodeReference("T:NS.Base"))
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test : Base", decompiled);
    }

    [Fact]
    public void Class_With_Interfaces()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            Interfaces = new TypeSignatureCollection
            {
                new VanillaTypeSignature("ICloneable", new CodeReference("T:System.ICloneable")),
                new GenericInstanceTypeSignature(new VanillaTypeSignature("IEquatable",
                    new CodeReference("T:System.IEquatable`1")))
                {
                    TypeArguments = new TypeSignatureCollection
                    {
                        new VanillaTypeSignature("Test", new CodeReference("T:NS.Test"))
                    }
                }
            }
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test : ICloneable, IEquatable<Test>", decompiled);
    }

    [Fact]
    public void Class_With_BaseType_And_Interfaces()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            BaseType = new VanillaTypeSignature("Base", new CodeReference("T:NS.Base")),
            Interfaces = new TypeSignatureCollection
            {
                new VanillaTypeSignature("ICloneable", new CodeReference("T:System.ICloneable")),
                new GenericInstanceTypeSignature(new VanillaTypeSignature("IEquatable",
                    new CodeReference("T:System.IEquatable`1")))
                {
                    TypeArguments = new TypeSignatureCollection
                    {
                        new VanillaTypeSignature("Test", new CodeReference("T:NS.Test"))
                    }
                }
            }
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test : Base, ICloneable, IEquatable<Test>", decompiled);
    }

    [Fact]
    public void Class_With_TypeParameters()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            TypeParameters = new TypeParameterSegmentCollection
            {
                new("T"),
                new("U")
            }
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test<T, U>", decompiled);
    }

    [Fact]
    public void Class_With_TypeParameter_Constraint()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            TypeParameters = new TypeParameterSegmentCollection
            {
                new("T")
                {
                    Constraints = new TypeArgumentConstraintCollection
                    {
                        new ValueTypeParameterConstraint(),
                        new InterfaceTypeParameterConstraint(new VanillaTypeSignature("ICloneable",
                            new CodeReference("T:System.ICloneable"))),
                    }
                }
            }
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test<T>\n    where T : struct, ICloneable", decompiled);
    }

    [Fact]
    public void Class_With_TypeParameter_Constraint2()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            TypeParameters = new TypeParameterSegmentCollection
            {
                new("T")
                {
                    Constraints = new TypeArgumentConstraintCollection
                    {
                        new ValueTypeParameterConstraint
                        {
                            IsUnmanaged = true
                        },
                    }
                }
            }
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test<T>\n    where T : unmanaged", decompiled);
    }

    [Fact]
    public void Class_With_TypeParameter_Constraint3()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            TypeParameters = new TypeParameterSegmentCollection
            {
                new("T")
                {
                    Constraints = new TypeArgumentConstraintCollection
                    {
                        new ReferenceTypeParameterConstraint
                        {
                            BaseType = new VanillaTypeSignature("Base", new CodeReference("T:Base"))
                        },
                        new InterfaceTypeParameterConstraint(new VanillaTypeSignature("ICloneable",
                            new CodeReference("T:System.ICloneable"))),
                    }
                },
                new("U")
                {
                    Constraints = new TypeArgumentConstraintCollection
                    {
                        new ReferenceTypeParameterConstraint
                        {
                            Nullability = NullabilityKind.Nullable
                        }
                    }
                }
            }
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public class Test<T, U>\n    where T : Base, ICloneable\n    where U : class?", decompiled);
    }

    [Theory]
    [InlineData(MemberVisibility.Public, "public")]
    [InlineData(MemberVisibility.Protected, "protected")]
    [InlineData(MemberVisibility.Private, "private")]
    [InlineData(MemberVisibility.Assembly, "internal")]
    [InlineData(MemberVisibility.ProtectedOrAssembly, "protected internal")]
    [InlineData(MemberVisibility.ProtectedAndAssembly, "private protected")]
    public void Class_Visibilities(MemberVisibility visibility, string expected)
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", visibility);

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal($"{expected} class Test", decompiled);
    }

    [Fact]
    public void Abstract_Class()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            IsAbstract = true
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public abstract class Test", decompiled);
    }

    [Fact]
    public void Sealed_Class()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            IsSealed = true
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public sealed class Test", decompiled);
    }

    [Fact]
    public void Static_Class()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var classDocumentation = new ClassDocumentation("Test", MemberVisibility.Public)
        {
            IsStatic = true
        };

        classDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public static class Test", decompiled);
    }

    [Fact]
    public void Interface()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var interfaceDocumentation = new InterfaceDocumentation("Test", MemberVisibility.Public);

        interfaceDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public interface Test", decompiled);
    }

    [Theory]
    [InlineData(TypeArgumentVarianceKind.Invariant, "")]
    [InlineData(TypeArgumentVarianceKind.Covariant, "out ")]
    [InlineData(TypeArgumentVarianceKind.Contravariant, "in ")]
    public void Interface_With_Variant_TypeParameter(TypeArgumentVarianceKind kind, string expected)
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var interfaceDocumentation = new InterfaceDocumentation("Test", MemberVisibility.Public)
        {
            TypeParameters = new TypeParameterSegmentCollection
            {
                new("T")
                {
                    Variance = kind
                }
            }
        };

        interfaceDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal($"public interface Test<{expected}T>", decompiled);
    }

    [Fact]
    public void Interface_With_Interfaces()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var interfaceDocumentation = new InterfaceDocumentation("Test", MemberVisibility.Public)
        {
            Interfaces = new TypeSignatureCollection
            {
                new VanillaTypeSignature("ICloneable", new CodeReference("T:System.ICloneable")),
                new VanillaTypeSignature("IDisposable", new CodeReference("T:System.IDisposable"))
            }
        };

        interfaceDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public interface Test : ICloneable, IDisposable", decompiled);
    }

    [Fact]
    public void Enum()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var enumDocumentation = new EnumDocumentation("Test", MemberVisibility.Public);

        enumDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public enum Test", decompiled);
    }

    [Fact]
    public void Enum_With_BaseType()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var enumDocumentation = new EnumDocumentation("Test", MemberVisibility.Public)
        {
            BaseType = new VanillaTypeSignature("UInt64", new CodeReference("T:System.UInt64"))
        };

        enumDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public enum Test : ulong", decompiled);
    }

    [Fact]
    public void Flags_Enum()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var enumDocumentation = new EnumDocumentation("Test", MemberVisibility.Public)
        {
            IsFlags = true
        };

        enumDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("[Flags]\npublic enum Test", decompiled);
    }

    [Fact]
    public void Struct()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var structDocumentation = new StructDocumentation("Test", MemberVisibility.Public);

        structDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public struct Test", decompiled);
    }

    [Fact]
    public void ReadOnly_Struct()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var structDocumentation = new StructDocumentation("Test", MemberVisibility.Public)
        {
            IsReadOnly = true
        };

        structDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public readonly struct Test", decompiled);
    }

    [Fact]
    public void Ref_Struct()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var structDocumentation = new StructDocumentation("Test", MemberVisibility.Public)
        {
            IsByRef = true
        };

        structDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public ref struct Test", decompiled);
    }

    [Fact]
    public void ReadOnly_Ref_Struct()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var structDocumentation = new StructDocumentation("Test", MemberVisibility.Public)
        {
            IsReadOnly = true,
            IsByRef = true
        };

        structDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public readonly ref struct Test", decompiled);
    }

    [Fact]
    public void Delegate()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var delegateDocumentation = new DelegateDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature("Void", new CodeReference("T:System.Void")));

        delegateDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public delegate void Test()", decompiled);
    }

    [Fact]
    public void Delegate_With_TypeParameters()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var delegateDocumentation = new DelegateDocumentation("Test", MemberVisibility.Public,
            new VanillaTypeSignature("Void", new CodeReference("T:System.Void")))
        {
            TypeParameters = new TypeParameterSegmentCollection
            {
                new("T")
                {
                    Constraints = new TypeArgumentConstraintCollection
                    {
                        new ValueTypeParameterConstraint()
                    }
                }
            }
        };
        delegateDocumentation.Parameters.Add(new ParameterSegment(new GenericParameterTypeSignature("T"), "param"));

        delegateDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public delegate void Test<T>(T param)\n    where T : struct", decompiled);
    }

    [Fact]
    public void Record()
    {
        var mediator = CreateMockMediator();
        var decompiler = new MemberDecompiler(mediator);
        var recordDocumentation = new RecordDocumentation("Test", MemberVisibility.Public)
        {
            TypeParameters = new TypeParameterSegmentCollection
            {
                new("T")
            },
            Parameters = new ParameterSegmentCollection
            {
                new(new VanillaTypeSignature("Int32", new CodeReference("T:System.Int32")), "First"),
                new(new GenericParameterTypeSignature("T"), "Second")
            }
        };

        recordDocumentation.AcceptVisitor(decompiler);
        string decompiled = decompiler.ToString();

        Assert.Equal("public record Test<T>(int First, T Second)", decompiled);
    }
}