using AsmResolver.DotNet;
using Doktr.Lifters.Common.DependencyGraph;

namespace Doktr.Lifters.AsmResolver.DependencyGraph;

public class DependencyGraphBuilder
{
    private readonly ModuleDefinition _module;
    private readonly DependencyGraph<IMemberDefinition> _depGraph = new();

    public DependencyGraphBuilder(ModuleDefinition module) => _module = module;

    public DependencyGraph<IMemberDefinition> BuildDependencyGraph()
    {
        AddTypes();

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
            AddEvent(ev);
        foreach (var field in type.Fields)
            AddField(field);
        foreach (var property in type.Properties)
            AddProperty(property);
        foreach (var method in type.Methods)
            AddMethod(method);
        foreach (var nestedType in type.NestedTypes)
            AddType(nestedType);
    }

    private void AddEvent(EventDefinition ev) => _depGraph.AddNode(ev);

    private void AddField(FieldDefinition field) => _depGraph.AddNode(field);

    private void AddProperty(PropertyDefinition property) => _depGraph.AddNode(property);

    private void AddMethod(MethodDefinition method) => _depGraph.AddNode(method);
}