using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Analysis.Transformations
{
    public class VirtualMethodTransformer : DependencyAdderTransformerBase
    {
        private static readonly SignatureComparer Comparer = new();

        protected override void Visit(DependencyNode node, TransformationContext context)
        {
            if (!(node is { MetadataMember: MethodDefinition method }))
                return;
            if (!method.IsVirtual)
                return;

            var state = new State(method.DeclaringType, method, context);

            if (Overriden(state) is { } overriden)
            {
                node.Dependencies.Add(overriden);
                CheckPropertyAndEvent(overriden);
            }

            if (ShadowedImplementation(state) is { } shadowed)
            {
                node.Dependencies.Add(shadowed);
                CheckPropertyAndEvent(shadowed);
            }
            else if (ExplicitImplementation(state) is { } @explicit)
            {
                node.Dependencies.Add(@explicit);
                CheckPropertyAndEvent(@explicit);
            }

            void CheckPropertyAndEvent(DependencyNode dependency)
            {
                if (!method.IsGetMethod && !method.IsSetMethod && !method.IsAddMethod && !method.IsRemoveMethod && !method.IsFireMethod)
                    return;
                
                if (dependency.Owner is not null)
                {
                    node.Owner.Dependencies.Add(dependency.Owner);
                }
                else
                {
                    var member = (IMethodDefOrRef) dependency.MetadataMember;
                    var needle = node.Owner.MetadataMember.Name;
                    var type = member.DeclaringType.Resolve();
                    if (method.IsGetMethod || method.IsSetMethod)
                    {
                        foreach (var property in type.Properties)
                        {
                            if (property.Name == needle)
                            {
                                node.Owner.Dependencies.Add(context.GetOrCreateNode(property));
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var @event in type.Events)
                        {
                            if (@event.Name == needle)
                            {
                                node.Owner.Dependencies.Add(context.GetOrCreateNode(@event));
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static DependencyNode ShadowedImplementation(in State state)
        {
            var needle = state.Method;

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var implementation in state.Type.Interfaces)
            {
                var @interface = implementation.Interface.Resolve();
                if (@interface is null)
                {
                    Logger.Warning($"Couldn't resolve the interface {implementation.Interface}");
                    continue;
                }

                foreach (var method in @interface.Methods)
                {
                    if (IsSameSignature(needle, method, CreateContext(implementation.Interface.ToTypeSignature())))
                    {
                        Logger.Verbose($"{needle} implicitly implements {method}");
                        return state.GetOrCreateNode(method);
                    }
                }
            }

            return null;
        }

        private static DependencyNode ExplicitImplementation(in State state)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var implementation in state.Type.MethodImplementations)
            {
                if (implementation.Body == state.Method)
                {
                    Logger.Verbose($"{state.Method} explicitly implements {implementation.Declaration}");
                    return state.GetOrCreateNode(implementation.Declaration);
                }
            }

            return null;
        }

        private static DependencyNode Overriden(in State state)
        {
            var needle = state.Method;
            var previous = state.Type;
            var type = state.Type.BaseType;

            while (type is not null)
            {
                var resolved = type.Resolve();
                if (resolved is null)
                {
                    Logger.Warning($"Couldn't resolve the base type of {previous}");
                    break;
                }

                foreach (var method in resolved.Methods)
                {
                    if (!method.IsVirtual)
                        continue;
                    if (!IsSameSignature(needle, method, CreateContext(type.ToTypeSignature())))
                        continue;
                    
                    Logger.Verbose($"{needle} overrides {method}");
                    return state.GetOrCreateNode(method);
                }

                previous = resolved;
                type = resolved.BaseType;
            }

            return null;
        }

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        private static bool IsSameSignature(MethodDefinition implementor, MethodDefinition @base, GenericContext context)
        {
            if (implementor.Name != @base.Name)
                return false;

            var inst = TryInstantiateGenericTypes(@base.Signature, context);
            return Comparer.Equals(implementor.Signature, inst);
        }

        private static MethodSignature TryInstantiateGenericTypes(MethodSignature signature, GenericContext context)
        {
            try
            {
                return signature.InstantiateGenericTypes(context);
            }
            catch
            {
                return signature;
            }
        }

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        private static GenericContext CreateContext(TypeSignature signature)
        {
            return signature is GenericInstanceTypeSignature generic
                ? new GenericContext().WithType(generic)
                : default;
        }

        private readonly struct State
        {
            internal State(TypeDefinition type, MethodDefinition method, TransformationContext context)
            {
                Type = type;
                Method = method;
                Context = context;
            }

            internal TypeDefinition Type
            {
                get;
            }

            internal MethodDefinition Method
            {
                get;
            }

            internal TransformationContext Context
            {
                get;
            }

            internal DependencyNode GetOrCreateNode(IFullNameProvider member) => Context.GetOrCreateNode(member);
        }
    }
}