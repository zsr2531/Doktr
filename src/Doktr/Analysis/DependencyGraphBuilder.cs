using System.Collections.Generic;
using System.Diagnostics;
using AsmResolver.DotNet;
using Doktr.Analysis.Transformations;

namespace Doktr.Analysis
{
    public class DependencyGraphBuilder
    {
        private readonly ModuleDefinition _module;
        private readonly Stack<DependencyNode> _agenda = new();
        private readonly Dictionary<INameProvider, DependencyNodeBase> _mapping = new();

        private static readonly ITransformer[] Transformers =
        {
            new PropertyTransformer(),
            new EventTransformer(),
            new BaseClassTransformer(),
            new InterfaceTransformer(),
            // new VirtualMethodTransformer()
        };

        public DependencyGraphBuilder(ModuleDefinition module)
        {
            _module = module;
        }

        public DependencyGraph BuildDependencyGraph()
        {
            var root = CreateNode(_module);
            _agenda.Push(root);
            
            foreach (var type in _module.TopLevelTypes)
                VisitType(type);

            // Sanity check:
            // We should never end up with more than 2 items on the stack at the end of the graph builder algorithm.
            // That would mean that the builder unfortunately contains a bug.
            Debug.Assert(_agenda.Count == 1);

            var context = new TransformationContext(_mapping);
            foreach (var transformer in Transformers)
                transformer.Transform(root, context);
            
            return new DependencyGraph(_mapping, root);
        }

        private void VisitType(TypeDefinition type)
        {
            var node = CreateNode(type);
            _agenda.Push(node);

            foreach (var nest in type.NestedTypes)
                VisitType(nest);
            
            foreach (var field in type.Fields)
                AddToAgenda(field);

            foreach (var @event in type.Events)
                AddToAgenda(@event);

            foreach (var property in type.Properties)
                AddToAgenda(property);
            
            foreach (var method in type.Methods)
                AddToAgenda(method);

            _agenda.Pop();
            _agenda.Peek().Children.Add(node);
        }

        private void AddToAgenda(INameProvider original) =>
            _agenda.Peek().Children.Add(CreateNode(original));

        private DependencyNode CreateNode(INameProvider original)
        {
            var node = new DependencyNode(original);
            _mapping[original] = node;

            return node;
        }
    }
}