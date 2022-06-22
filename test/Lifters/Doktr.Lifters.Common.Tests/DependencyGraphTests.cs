using Doktr.Lifters.Common.DependencyGraph;
using FluentAssertions;
using Xunit;

namespace Doktr.Lifters.Common.Tests;

public class DependencyGraphTests
{
    [Fact]
    public void Node_Mapping()
    {
        var depGraph = new DependencyGraph<int>();
        var node = depGraph.AddNode(1);

        depGraph.Nodes.Count.Should().Be(1);
        depGraph.NodeMap[1].Should().Be(node);
    }

    [Fact]
    public void Duplicate_Values_Should_Result_In_One_Node()
    {
        var depGraph = new DependencyGraph<int>();
        var node1 = depGraph.AddNode(1);
        var node2 = depGraph.AddNode(1);

        depGraph.Nodes.Count.Should().Be(1);
        node1.Should().Be(node2);
    }

    [Fact]
    public void Dependencies_Added_To_Nodes()
    {
        var depGraph = new DependencyGraph<int>();
        var node1 = depGraph.AddNode(1);
        var node2 = depGraph.AddNode(2);

        depGraph.AddDependency(node1, node2, DependencyEdgeKind.Extension);

        depGraph.Nodes.Count.Should().Be(2);
        node1.GetDependencies().Should().ContainSingle().Which.To.Value.Should().Be(2);
        node2.GetDependants().Should().ContainSingle().Which.From.Value.Should().Be(1);
    }
}