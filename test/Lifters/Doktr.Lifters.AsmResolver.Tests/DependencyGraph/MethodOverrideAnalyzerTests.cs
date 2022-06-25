using Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;
using Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases;
using Doktr.Lifters.Common.DependencyGraph;
using FluentAssertions;
using Xunit;

namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph;

public class MethodOverrideAnalyzerTests : IClassFixture<AnalyzerFixture<MethodOverrideAnalyzer>>
{
    private readonly AnalyzerFixture<MethodOverrideAnalyzer> _fixture;

    public MethodOverrideAnalyzerTests(AnalyzerFixture<MethodOverrideAnalyzer> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Abstract_Class_Results_In_No_Warnings()
    {
        _fixture.AnalyzeNode(typeof(AbstractClass).GetMethod("AbstractMethod")!);
        _fixture.AnalyzeNode(typeof(AbstractClass).GetMethod("VirtualMethod")!);
    }

    [Fact]
    public void Abstract_Method_Implementation_Should_Link_To_Definition()
    {
        var node = _fixture.AnalyzeNode(typeof(SubClass).GetMethod("AbstractMethod")!);
        var abs = _fixture.GetNodeFor(typeof(AbstractClass).GetMethod("AbstractMethod")!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == abs && e.Kind == DependencyEdgeKind.Override);
        abs.GetDependants().Should()
           .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Override);
    }

    [Fact]
    public void SubSubClass_Should_Link_To_Abstract_Class()
    {
        var node = _fixture.AnalyzeNode(typeof(SubSubClass).GetMethod("VirtualMethod")!);
        var vir = _fixture.GetNodeFor(typeof(AbstractClass).GetMethod("VirtualMethod")!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == vir && e.Kind == DependencyEdgeKind.Override);
        vir.GetDependants().Should()
           .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.Override);
    }
}