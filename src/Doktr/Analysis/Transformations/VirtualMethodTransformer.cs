using System;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;

namespace Doktr.Analysis.Transformations
{
    public class VirtualMethodTransformer : DependencyAdderTransformerBase
    {
        private static readonly SignatureComparer Comparer = new SignatureComparer();
        
        protected override void Visit(DependencyNodeBase node, TransformationContext context)
        {
            if (!(node is DependencyNode { MetadataMember: MethodDefinition method }))
                return;
            if (!method.IsVirtual)
                return;
            
            var other = new GenericContext();
            var internalContext = new InternalContext(method.DeclaringType, method, context);
            
            if (ExplicitImplementation(internalContext) is {} @explicit)
                node.Dependencies.Add(@explicit);
            else if (ShadowedImplementation(internalContext) is {} shadowed)
                node.Dependencies.Add(shadowed);
            else if (Overriden(internalContext) is {} overriden)
                node.Dependencies.Add(overriden);
            else
                throw new InvalidOperationException();
        }

        private static DependencyNodeBase ShadowedImplementation(in InternalContext context)
        {
            var needle = context.Method;
            
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var implementation in context.Type.Interfaces)
            {
                var @interface = implementation.Interface.Resolve();
                foreach (var method in @interface.Methods)
                {
                    if (method.Name == needle.Name && Comparer.Equals(method.Signature, needle.Signature))
                        return context.GetOrCreateNode(method);
                }
            }

            return null;
        }
        
        private static DependencyNodeBase ExplicitImplementation(in InternalContext context)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var implementation in context.Type.MethodImplementations)
            {
                if (implementation.Body == context.Method)
                    return context.GetOrCreateNode(implementation.Declaration);
            }

            return null;
        }

        private static DependencyNodeBase Overriden(in InternalContext context)
        {
            var needle = context.Method;
            var type = context.Type.DeclaringType;
            
            while (type is not null)
            {
                foreach (var method in type.Methods)
                {
                    if (!method.IsVirtual)
                        continue;
                    if (method.Name != needle.Name && Comparer.Equals(method.Signature, needle.Signature))
                        continue;

                    return context.GetOrCreateNode(method);
                }
                
                type = type.DeclaringType;
            }

            return null;
        }

        private readonly struct InternalContext
        {
            internal InternalContext(TypeDefinition type, MethodDefinition method, TransformationContext context)
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

            internal DependencyNodeBase GetOrCreateNode(INameProvider member) => Context.GetOrCreateNode(member);
        }
    }
}