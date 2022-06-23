using Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;
using Doktr.Lifters.AsmResolver.Tests.TestCases;
using Doktr.Lifters.Common.DependencyGraph;
using FluentAssertions;
using Xunit;

namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph;

public class BaseTypeAnalyzerTests : IClassFixture<AnalyzerFixture<BaseTypeAnalyzer>>
{
    private readonly AnalyzerFixture<BaseTypeAnalyzer> _fixture;

    public BaseTypeAnalyzerTests(AnalyzerFixture<BaseTypeAnalyzer> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Normal_Class_Should_Have_Object_As_Base()
    {
        var node = _fixture.AnalyzeNode(typeof(AbstractClass));
        var obj = _fixture.GetNodeFor(typeof(object));

        node.GetDependencies().Should().ContainSingle(e => e.To == obj && e.Kind == DependencyEdgeKind.Extension);
        obj.GetDependants().Should().ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Extension);
    }

    [Fact]
    public void SubClass_Should_Have_SuperClass_As_Base()
    {
        var node = _fixture.AnalyzeNode(typeof(SubClass));
        var super = _fixture.GetNodeFor(typeof(AbstractClass));

        node.GetDependencies().Should().ContainSingle(e => e.To == super && e.Kind == DependencyEdgeKind.Extension);
        super.GetDependants().Should().ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Extension);
    }

    [Fact]
    public void Class_With_Interface_Should_Implement_The_Interface()
    {
        var node = _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var inf = _fixture.GetNodeFor(typeof(IInterface));

        node.GetDependencies().Should().ContainSingle(e => e.To == inf && e.Kind == DependencyEdgeKind.Implementation);
        inf.GetDependants().Should().ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Implementation);
    }

    [Fact]
    public void SubClass_With_Interface_Should_Have_Base_And_Implement_The_Interface()
    {
        var node = _fixture.AnalyzeNode(typeof(SubClassWithInterface));
        var super = _fixture.GetNodeFor(typeof(ClassWithInterface));
        var inf = _fixture.GetNodeFor(typeof(IInterface));

        super.GetDependants().Should().ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Extension);
        inf.GetDependants().Should().ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Implementation);
        node.GetDependencies().Should().HaveCount(2)
            .And.OnlyContain(e => e.To == super || e.To == inf);
    }
}