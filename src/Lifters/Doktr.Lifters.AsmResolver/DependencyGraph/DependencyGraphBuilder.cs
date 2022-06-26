using AsmResolver.DotNet;
using Doktr.Lifters.Common.DependencyGraph;

namespace Doktr.Lifters.AsmResolver.DependencyGraph;

public class DependencyGraphBuilder : IDependencyGraphBuilder<IMemberDefinition>
{
    private readonly IEnumerable<IDependencyGraphAnalyzer<IMemberDefinition>> _analyzers;
    private readonly IEnumerable<ModuleDefinition> _modules;
    private readonly DependencyGraph<IMemberDefinition> _depGraph = new();

    public DependencyGraphBuilder(
        IEnumerable<IDependencyGraphAnalyzer<IMemberDefinition>> analyzers,
        IEnumerable<ModuleDefinition> modules)
    {
        _analyzers = analyzers;
        _modules = modules;
    }

    public DependencyGraph<IMemberDefinition> BuildDependencyGraph()
    {
        InspectModules();
        return _depGraph;
    }

    public void PerformAnalysis()
    {
        foreach (var node in _depGraph.Nodes)
            AnalyzeNode(node);
    }

    private void InspectModules()
    {
        foreach (var module in _modules)
            InspectTypes(module);
    }

    private void InspectTypes(ModuleDefinition module)
    {
        foreach (var type in module.TopLevelTypes)
            InspectType(type);
    }

    private void InspectType(TypeDefinition type)
    {
        AddMember(type);
        foreach (var ev in type.Events)
            AddMember(ev);
        foreach (var field in type.Fields)
            AddMember(field);
        foreach (var property in type.Properties)
            AddMember(property);
        foreach (var method in type.Methods)
            AddMember(method);
        foreach (var nestedType in type.NestedTypes)
            InspectType(nestedType);
    }

    private void AddMember(IMemberDefinition member) => _depGraph.AddNode(member);

    private void AnalyzeNode(DependencyNode<IMemberDefinition> node)
    {
        foreach (var analyzer in _analyzers)
            analyzer.AnalyzeNode(node);
    }
}