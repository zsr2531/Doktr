using System.Reflection;
using Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;
using Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases;
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
        const string fullName = "Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases.IInterface.ExplicitMethod";
        var impl = typeof(ClassWithInterface).GetMethod(fullName, (BindingFlags) (-1))!;

        _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var node = _fixture.GetNodeFor(impl);
        var decl = _fixture.GetNodeFor(typeof(IInterface).GetMethod("ExplicitMethod")!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == decl && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        decl.GetDependants().Should()
            .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.ExplicitImplementation);
    }

    [Fact]
    public void Explicit_Property_Should_Link_Both_Property_And_Accessor()
    {
        const string fullName = "Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases.IInterface.ExplicitProperty";
        var impl = typeof(ClassWithInterface).GetProperty(fullName, (BindingFlags) (-1))!;

        _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var node = _fixture.GetNodeFor(impl);
        var get = _fixture.GetNodeFor(impl.GetMethod!);
        var declProp = typeof(IInterface).GetProperty("ExplicitProperty")!;
        var decl = _fixture.GetNodeFor(declProp);
        var declGet = _fixture.GetNodeFor(declProp.GetMethod!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == decl && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        decl.GetDependants().Should()
            .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        get.GetDependencies().Should()
           .ContainSingle(e => e.To == declGet && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        declGet.GetDependants().Should()
               .ContainSingle(e => e.From == get && e.Kind == DependencyEdgeKind.ExplicitImplementation);
    }

    [Fact]
    public void Explicit_Event_Should_Link_Both_Event_And_Accessors()
    {
        const string fullName = "Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases.IInterface.ExplicitEvent";
        var impl = typeof(ClassWithInterface).GetEvent(fullName, (BindingFlags) (-1))!;

        _fixture.AnalyzeNode(typeof(ClassWithInterface));
        var node = _fixture.GetNodeFor(impl);
        var add = _fixture.GetNodeFor(impl.AddMethod!);
        var remove = _fixture.GetNodeFor(impl.RemoveMethod!);
        var declProp = typeof(IInterface).GetEvent("ExplicitEvent")!;
        var decl = _fixture.GetNodeFor(declProp);
        var declAdd = _fixture.GetNodeFor(declProp.AddMethod!);
        var declRemove = _fixture.GetNodeFor(declProp.RemoveMethod!);

        node.GetDependencies().Should()
            .ContainSingle(e => e.To == decl && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        decl.GetDependants().Should()
            .ContainSingle(e => e.From == node && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        add.GetDependencies().Should()
           .ContainSingle(e => e.To == declAdd && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        declAdd.GetDependants().Should()
               .ContainSingle(e => e.From == add && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        remove.GetDependencies().Should()
              .ContainSingle(e => e.To == declRemove && e.Kind == DependencyEdgeKind.ExplicitImplementation);
        declRemove.GetDependants().Should()
                  .ContainSingle(e => e.From == remove && e.Kind == DependencyEdgeKind.ExplicitImplementation);
    }
}