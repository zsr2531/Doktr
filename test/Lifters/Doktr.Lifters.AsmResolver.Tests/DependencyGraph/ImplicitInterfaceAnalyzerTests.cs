using System;
using Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;
using Doktr.Lifters.AsmResolver.Tests.TestCases;
using Doktr.Lifters.Common.DependencyGraph;
using FluentAssertions;
using Xunit;

namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph;

public class ImplicitInterfaceAnalyzerTests : IClassFixture<AnalyzerFixture<ImplicitInterfaceAnalyzer>>
{
    private readonly AnalyzerFixture<ImplicitInterfaceAnalyzer> _fixture;

    public ImplicitInterfaceAnalyzerTests(AnalyzerFixture<ImplicitInterfaceAnalyzer> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Normal_Method()
    {
        _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var node = _fixture.GetNodeFor(typeof(ClassWithInterface).GetMethod("ImplicitMethod")!);
        var inf = _fixture.GetNodeFor(typeof(IInterface).GetMethod("ImplicitMethod")!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == inf && e.Kind == DependencyEdgeKind.Implementation);
        inf.GetDependants().Should()
           .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Implementation);
    }

    [Fact]
    public void Generic_Method()
    {
        _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var node = _fixture.GetNodeFor(typeof(ClassWithInterface).GetMethod("ImplicitGenericMethodWithParam")!);
        var inf = _fixture.GetNodeFor(typeof(IInterface).GetMethod("ImplicitGenericMethodWithParam")!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == inf && e.Kind == DependencyEdgeKind.Implementation);
        inf.GetDependants().Should()
           .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Implementation);
    }

    [Fact]
    public void Generic_Method_Through_Generic_Interface()
    {
        _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var node = _fixture.GetNodeFor(typeof(ClassWithInterface).GetMethod("ImplicitGenericMethod")!);
        var inf = _fixture.GetNodeFor(typeof(IGenericInterface<>).GetMethod("ImplicitGenericMethod")!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == inf && e.Kind == DependencyEdgeKind.Implementation);
        inf.GetDependants().Should()
           .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Implementation);
    }
}