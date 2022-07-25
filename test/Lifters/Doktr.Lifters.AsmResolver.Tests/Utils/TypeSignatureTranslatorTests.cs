using System.Reflection;
using AsmResolver.DotNet;
using Doktr.Lifters.AsmResolver.Tests.Utils.TestCases;
using Doktr.Lifters.AsmResolver.Utils;
using FluentAssertions;
using NSubstitute;
using Serilog;
using Xunit;

namespace Doktr.Lifters.AsmResolver.Tests.Utils;

public class TypeSignatureTranslatorTests : IClassFixture<ModuleFixture<TypeSignatureTranslatorTests>>
{
    private readonly ModuleFixture<TypeSignatureTranslatorTests> _fixture;

    public TypeSignatureTranslatorTests(ModuleFixture<TypeSignatureTranslatorTests> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void NormalType()
    {
        string result = TranslateField("A");
        result.Should().Be("System.String");
    }

    [Fact]
    public void Nullable_Int()
    {
        string result = TranslateField("B");
        result.Should().Be("System.Nullable{System.Int32}");
    }

    [Fact]
    public void Nested_Generics()
    {
        string result = TranslateField("C");
        result.Should()
              .Be("System.Collections.Generic.List{System.Collections.Generic.Dictionary{System.Int32,System.String}}");
    }

    [Fact]
    public void Generics_With_Nested_Classes()
    {
        string result = TranslateField("D");
        result.Should()
              .Be("Doktr.Lifters.AsmResolver.Tests.Utils.TestCases.TypeSignatureTranslatorCases." +
                  "OuterClass{System.Int32}.InnerClass{System.Single,System.Double}");
    }

    [Fact]
    public void SzArray()
    {
        string result = TranslateField("E");
        result.Should().Be("System.Int32[]");
    }

    [Fact]
    public void Jagged_Array()
    {
        string result = TranslateField("F");
        result.Should().Be("System.Int32[0:,0:,0:]");
    }

    [Fact]
    public void Pointer()
    {
        string result = TranslateField("G");
        result.Should().Be("System.Double*");
    }

    [Fact]
    public void ByRef()
    {
        var method = (MethodDefinition) _fixture.GetMember(typeof(TypeSignatureTranslatorCases).GetMethod("I")!);
        var param = method.Parameters[0];
        var signature = param.ParameterType;
        var translator = new TypeSignatureTranslator(Substitute.For<ILogger>());

        string result = signature.AcceptVisitor(translator);

        result.Should().Be("System.Int32@");
    }

    [Fact]
    public void Function_Pointer()
    {
        var field = (FieldDefinition) _fixture.GetMember(typeof(TypeSignatureTranslatorCases).GetField("H")!);
        var signature = field.Signature!.FieldType;
        var logger = Substitute.For<ILogger>();
        var translator = new TypeSignatureTranslator(logger);

        string result = signature.AcceptVisitor(translator);

        result.Should().BeEmpty();
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        logger.ReceivedWithAnyArgs().Warning(Arg.Any<string>());
    }

    private string TranslateField(string fieldName)
    {
        var member = (FieldDefinition) _fixture.GetMember(typeof(TypeSignatureTranslatorCases).GetField(fieldName)!);
        var translator = new TypeSignatureTranslator(Substitute.For<ILogger>());

        return member.Signature!.FieldType.AcceptVisitor(translator);
    }
}