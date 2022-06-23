using System.Reflection;
using Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;
using Doktr.Lifters.AsmResolver.Tests.TestCases;
using Doktr.Lifters.Common.DependencyGraph;
using FluentAssertions;
using Xunit;

namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph;

public class ExplicitInterfaceAnalyzerTests : IClassFixture<AnalyzerFixture<ExplicitInterfaceAnalyzer>>
{
    private readonly AnalyzerFixture<ExplicitInterfaceAnalyzer> _fixture;

    public ExplicitInterfaceAnalyzerTests(AnalyzerFixture<ExplicitInterfaceAnalyzer> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Explicit_Implementation_Should_Link_Back_To_Interface()
    {
        const string fullName = "Doktr.Lifters.AsmResolver.Tests.TestCases.IInterface.Explicit";
        var impl = typeof(ClassWithInterface).GetMethod(fullName, (BindingFlags) (-1))!;

        _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var node = _fixture.GetNodeFor(impl);
        var decl = _fixture.GetNodeFor(typeof(IInterface).GetMethod("Explicit")!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == decl && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        decl.GetDependants().Should()
            .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.ExplicitImplementation);
    }
}