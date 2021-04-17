using AsmResolver.DotNet;

namespace Doktr.Analysis.Transformations
{
    public class BaseClassTransformer : DependencyAdderTransformerBase
    {
        protected override void Visit(DependencyNode node, TransformationContext context)
        {
            if (!(node.MetadataMember is TypeDefinition type))
                return;

            if (type.BaseType is null)
                return;
            
            var super = type.BaseType.Resolve();
            if (super is null)
            {
                Logger.Warning($"Couldn't resolve the base class of {type}");
                return;
            }
            
            var superNode = context.GetOrCreateNode(super);
            
            Depend(node, superNode);
        }
    }
}