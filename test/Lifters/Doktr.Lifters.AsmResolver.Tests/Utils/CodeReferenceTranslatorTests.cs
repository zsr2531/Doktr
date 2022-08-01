using AsmResolver.DotNet;
using Doktr.Core.Models;
using Doktr.Lifters.AsmResolver.Tests.Utils.TestCases;
using Doktr.Lifters.AsmResolver.Utils;
using FluentAssertions;
using NSubstitute;
using Serilog;
using Xunit;

namespace Doktr.Lifters.AsmResolver.Tests.Utils;

public class CodeReferenceTranslatorTests : IClassFixture<ModuleFixture<CodeReferenceTranslatorCases>>
{
    private readonly ModuleFixture<CodeReferenceTranslatorCases> _fixture;

    public CodeReferenceTranslatorTests(ModuleFixture<CodeReferenceTranslatorCases> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Type()
    {
        var type = _fixture.GetMember(typeof(CodeReferenceTranslatorCases));
        var codeRef = TranslateMember(type);

        codeRef.IsType.Should().BeTrue();
        codeRef.Name.ToString().Should().Be(type.FullName);
    }

    [Fact]
    public void Event()
    {
        var ev = _fixture.GetMember(typeof(CodeReferenceTranslatorCases).GetEvent("Event")!);
        var codeRef = TranslateMember(ev);

        codeRef.IsEvent.Should().BeTrue();
        CheckName(codeRef, ev);
    }

    [Fact]
    public void Property()
    {
        var prop = _fixture.GetMember(typeof(CodeReferenceTranslatorCases).GetProperty("Property")!);
        var codeRef = TranslateMember(prop);

        codeRef.IsProperty.Should().BeTrue();
        CheckName(codeRef, prop);
    }

    [Fact]
    public void Method()
    {
        var method = _fixture.GetMember(typeof(CodeReferenceTranslatorCases).GetMethod("Method")!);
        var codeRef = TranslateMember(method);

        codeRef.IsMethod.Should().BeTrue();
        codeRef.Name.ToString().Should()
               .Be(typeof(CodeReferenceTranslatorCases).FullName + ".Method(System.String)");
    }

    [Fact]
    public void Method_With_Multiple_Parameters()
    {
        var method = _fixture.GetMember(typeof(CodeReferenceTranslatorCases).GetMethod("MethodWithTwoParams")!);
        var codeRef = TranslateMember(method);

        codeRef.IsMethod.Should().BeTrue();
        codeRef.Name.ToString().Should()
               .Be(typeof(CodeReferenceTranslatorCases).FullName + ".MethodWithTwoParams(System.Int32,System.Int32)");
    }

    [Fact]
    public void GenericInnerClass()
    {
        var inner = _fixture.GetMember(typeof(CodeReferenceTranslatorCases).GetNestedType("GenericInnerClass`1")!);
        var codeRef = TranslateMember(inner);

        codeRef.IsType.Should().BeTrue();
        codeRef.Name.ToString().Should()
               .Be(typeof(CodeReferenceTranslatorCases).FullName + ".GenericInnerClass`1");
    }

    [Fact]
    public void GenericInnerInnerClass()
    {
        var inner = _fixture.GetMember(typeof(CodeReferenceTranslatorCases)
                                       .GetNestedType("GenericInnerClass`1")!
                                       .GetNestedType("GenericInnerInnerClass`1")!);
        var codeRef = TranslateMember(inner);

        codeRef.IsType.Should().BeTrue();
        codeRef.Name.ToString().Should()
               .Be(typeof(CodeReferenceTranslatorCases).FullName + ".GenericInnerClass`1.GenericInnerInnerClass`1");
    }

    private static CodeReference TranslateMember(IMemberDefinition member)
    {
        var signatureTranslator = new TypeSignatureTranslator(Substitute.For<ILogger>());
        var codeRefTranslator = new CodeReferenceTranslator(signatureTranslator);

        return codeRefTranslator.TranslateMember(member);
    }

    private static void CheckName(CodeReference codeRef, IMemberDefinition member)
    {
        string type = member.DeclaringType is null
            ? ""
            : member.DeclaringType.FullName + '.';
        string expected = type + member.Name;

        codeRef.Name.ToString().Should().Be(expected);
    }
}