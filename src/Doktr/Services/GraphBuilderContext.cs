using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Doktr.Services.GraphTransformers;

namespace Doktr.Services;

public class GraphBuilderContext
{
    private readonly ImmutableDictionary<IFullNameProvider, DependencyNode>.Builder _mapping;

    public GraphBuilderContext(ImmutableDictionary<IFullNameProvider, DependencyNode>.Builder mapping)
    {
        _mapping = mapping;
    }

    public void AcceptTransformer(IDependencyGraphTransformer transformer)
    {
        foreach (var (_, node) in _mapping)
            transformer.VisitNode(node, this);
    }

    public DependencyNode GetOrCreateNode(IFullNameProvider metadataMember, DependencyNode? parent = null)
    {
        if (_mapping.TryGetValue(metadataMember, out var node))
            return node;

        node = new DependencyNode(metadataMember, parent);
        _mapping[metadataMember] = node;
        return node;
    }

    public bool NodeExists(IFullNameProvider metadataMember) => _mapping.ContainsKey(metadataMember);

    public void AddDependency(IFullNameProvider from, IFullNameProvider to)
    {
        if (from is MethodDefinition { Semantics: { Association: { } fromAssociation } }
            && to is MethodDefinition { Semantics: { Association: { } toAssociation } })
        {
            AddDependency(fromAssociation, toAssociation);
        }

        GetOrCreateNode(from).Dependencies.Add(GetOrCreateNode(to));
    }
}