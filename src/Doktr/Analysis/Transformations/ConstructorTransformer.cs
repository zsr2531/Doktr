using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;

namespace Doktr.Analysis.Transformations
{
    public class ConstructorTransformer : DependencyAdderTransformerBase
    {
        private static readonly SignatureComparer Comparer = new();
        
        protected override void Visit(DependencyNode node, TransformationContext context)
        {
            if (node.MetadataMember is not MethodDefinition method)
                return;
            if (!method.IsConstructor)
                return;

            var previous = method.DeclaringType;
            var type = method.DeclaringType.BaseType;

            while (type is not null)
            {
                var resolved = type.Resolve();
                if (resolved is null)
                {
                    Logger.Warning($"Couldn't resolve the base type of {previous}");
                    break;
                }

                var generic = CreateContext(type.ToTypeSignature());
                foreach (var ctor in resolved.Methods.Where(m => m.IsConstructor))
                {
                    if (!IsSameSignature(method, ctor, generic))
                        continue;
                    
                    node.Dependencies.Add(context.GetOrCreateNode(ctor));
                    break;
                }

                previous = resolved;
                type = type.DeclaringType;
            }
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
    }
}