using System;
using System.Linq;
using System.Reflection;
using AsmResolver.DotNet;
using Doktr.Lifters.AsmResolver.DependencyGraph;
using Doktr.Lifters.Common.DependencyGraph;
using FluentAssertions;
using NSubstitute;
using Serilog;

namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph;

public class AnalyzerFixture<T> : ModuleFixture<AnalyzerFixture<T>>
    where T : IDependencyGraphAnalyzer<IMemberDefinition>
{
    public AnalyzerFixture()
    {
        DependencyGraph = BuildDependencyGraph();
        Analyzer = (T) Activator.CreateInstance(typeof(T), Logger)!;
    }

    public ILogger Logger { get; } = Substitute.For<ILogger>();
    public DependencyGraph<IMemberDefinition> DependencyGraph { get; }
    public T Analyzer { get; }

    public DependencyNode<IMemberDefinition> GetNodeFor(MemberInfo member)
    {
        return DependencyGraph.Nodes.Single(m => m.Value.MetadataToken == member.MetadataToken);
    }

    public DependencyNode<IMemberDefinition> AnalyzeNode(MemberInfo member)
    {
        var node = GetNodeFor(member);
        Analyzer.AnalyzeNode(node);
        AssertNoWarnings();

        return node;
    }

    public void AssertNoWarnings()
    {
        int warnings = Logger
                       .ReceivedCalls()
                       .Count(c => c.GetMethodInfo().Name is "Warning" or "Error" or "Fatal");

        warnings.Should().Be(0);
    }

    private DependencyGraph<IMemberDefinition> BuildDependencyGraph()
    {
        var builder = new DependencyGraphBuilder(
            Array.Empty<IDependencyGraphAnalyzer<IMemberDefinition>>(), new[] { Module });
        return builder.BuildDependencyGraph();
    }

}