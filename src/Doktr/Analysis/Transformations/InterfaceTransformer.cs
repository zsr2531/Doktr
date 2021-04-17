using AsmResolver.DotNet;

namespace Doktr.Analysis.Transformations
{
    public class InterfaceTransformer : DependencyAdderTransformerBase
    {
        protected override void Visit(DependencyNode node, TransformationContext context)
        {
            if (!(node.MetadataMember is TypeDefinition type))
                return;

            var interfaces = type.Interfaces;
            foreach (var @interface in interfaces)
            {
                var resolved = @interface.Interface.Resolve();
                if (resolved is null)
                {
                    Logger.Warning($"Couldn't resolve the interface {@interface}");
                    return;
                }
                
                var resolvedNode = context.GetOrCreateNode(resolved);
                
                Depend(node, resolvedNode);
            }
        }
    }
}