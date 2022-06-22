using System;
using System.Reflection;
using Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;
using Doktr.Lifters.AsmResolver.Tests.TestCases;
using Doktr.Lifters.Common.DependencyGraph;
using FluentAssertions;
using Xunit;

namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph;

public class ConstructorAnalyzerTests : IClassFixture<AnalyzerFixture<ConstructorAnalyzer>>
{
    private readonly AnalyzerFixture<ConstructorAnalyzer> _fixture;

    public ConstructorAnalyzerTests(AnalyzerFixture<ConstructorAnalyzer> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Constructor_In_Same_Class()
    {
        var node = _fixture.AnalyzeNode(typeof(SubClass).GetConstructor(Type.EmptyTypes)!);
        var otherCtor = _fixture.GetNodeFor(typeof(SubClass).GetConstructor(new[] { typeof(int) })!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == otherCtor && e.Kind == DependencyEdgeKind.OtherConstructor);
        otherCtor.GetDependants().Should()
            .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.OtherConstructor);
    }

    [Fact]
    public void Constructor_To_Base_Class()
    {
        var node = _fixture.AnalyzeNode(typeof(SubClass).GetConstructor(new[] { typeof(int) })!);
        var otherCtor = _fixture.GetNodeFor(typeof(AbstractClass).GetConstructor((BindingFlags) (-1), Type.EmptyTypes)!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == otherCtor && e.Kind == DependencyEdgeKind.OtherConstructor);
        otherCtor.GetDependants().Should()
            .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.OtherConstructor);
    }
}