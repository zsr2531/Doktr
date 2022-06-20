using AsmResolver.DotNet;
using Doktr.Lifters.Common.DependencyGraph;

namespace Doktr.Lifters.AsmResolver.DependencyGraph;

public class DependencyGraphBuilder : IDependencyGraphBuilder<IMemberDefinition>
{
    private readonly List<IDependencyGraphAnalyzer<IMemberDefinition>> _analyzers;
    private readonly ModuleDefinition _module;
    private readonly DependencyGraph<IMemberDefinition> _depGraph = new();

    public DependencyGraphBuilder(
        IEnumerable<IDependencyGraphAnalyzer<IMemberDefinition>> analyzers,
        ModuleDefinition module)
    {
        _analyzers = analyzers.ToList();
        _module = module;
    }

    public DependencyGraph<IMemberDefinition> BuildDependencyGraph()
    {
        AddTypes();
        PerformAnalysis();

        return _depGraph;
    }

    private void AddTypes()
    {
        foreach (var type in _module.TopLevelTypes)
            AddType(type);
    }

    private void AddType(TypeDefinition type)
    {
        _depGraph.AddNode(type);
        foreach (var ev in type.Events)
            AddMember(ev);
        foreach (var field in type.Fields)
            AddMember(field);
        foreach (var property in type.Properties)
            AddMember(property);
        foreach (var method in type.Methods)
            AddMember(method);
        foreach (var nestedType in type.NestedTypes)
            AddType(nestedType);
    }

    private void AddMember(IMemberDefinition member) => _depGraph.AddNode(member);

    private void PerformAnalysis()
    {
        foreach (var node in _depGraph.Nodes)
            AnalyzeNode(node);
    }

    private void AnalyzeNode(DependencyNode<IMemberDefinition> node)
    {
        foreach (var analyzer in _analyzers)
            analyzer.AnalyzeNode(node);
    }
}