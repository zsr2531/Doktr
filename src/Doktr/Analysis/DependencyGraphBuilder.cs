using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Doktr.Analysis.Transformations;

namespace Doktr.Analysis
{
    public class DependencyGraphBuilder
    {
        private readonly ModuleDefinition _module;
        private readonly Stack<DependencyNode> _agenda = new();
        private readonly Dictionary<IFullNameProvider, DependencyNode> _mapping = new(FullNameProviderComparer.Instance);

        private static readonly ITransformer[] Transformers =
        {
            new PropertyTransformer(),
            new EventTransformer(),
            new BaseClassTransformer(),
            new InterfaceTransformer(),
            new VirtualMethodTransformer(),
            new ConstructorTransformer()
        };

        public DependencyGraphBuilder(ModuleDefinition module)
        {
            _module = module;
        }

        public DependencyGraph BuildDependencyGraph()
        {
            var root = CreateNode(_module.Assembly);
            _agenda.Push(root);
            
            foreach (var type in _module.TopLevelTypes)
                VisitType(type);

            // Sanity check:
            // We should never end up with more than 1 item on the stack at the end of the graph builder algorithm.
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

            foreach (var nest in type.NestedTypes.Where(t => t.IsPublic || t.IsNestedFamily || t.IsNestedFamilyOrAssembly))
                VisitType(nest);
            
            foreach (var field in type.Fields.Where(f => f.IsPublic || f.IsFamilyOrAssembly || f.IsFamily))
                AddToAgenda(field);

            foreach (var @event in type.Events.Where(e => NeedsProcessing(GetDominantSemanticMethod(e))))
                AddToAgenda(@event);

            foreach (var property in type.Properties.Where(p => NeedsProcessing(GetDominantSemanticMethod(p))))
                AddToAgenda(property);
            
            foreach (var method in type.Methods.Where(NeedsProcessing))
                AddToAgenda(method);

            _agenda.Pop();
            _agenda.Peek().Children.Add(node);
        }

        private void AddToAgenda(IFullNameProvider original) =>
            _agenda.Peek().Children.Add(CreateNode(original));

        private DependencyNode CreateNode(IFullNameProvider original)
        {
            var node = new DependencyNode(original);
            _mapping[original] = node;

            return node;
        }

        private static bool NeedsProcessing(MethodDefinition method)
        {
            return method.IsPublic || method.IsFamilyOrAssembly || method.IsFamily;
        }

        // From AsmResolver workspaces.
        private static MethodDefinition GetDominantSemanticMethod(IHasSemantics subject)
        {
            MethodDefinition dominantMethod = null;
            var maxAccessibility = MethodAttributes.CompilerControlled;

            for (int i = 0; i < subject.Semantics.Count; i++)
            {
                var method = subject.Semantics[i].Method;

                // Check if accessibility is less restrictive than the previously found one.
                var accessibility = method.Attributes & MethodAttributes.MemberAccessMask;
                if (maxAccessibility < accessibility)
                {
                    dominantMethod = method;
                    maxAccessibility = accessibility;
                }
            }

            return dominantMethod;
        }
    }
}