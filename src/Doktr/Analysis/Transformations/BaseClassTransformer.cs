using AsmResolver.DotNet;

namespace Doktr.Analysis.Transformations
{
    public class BaseClassTransformer : DependencyAdderTransformerBase
    {
        protected override void Visit(DependencyNodeBase node, TransformationContext context)
        {
            if (!(node.MetadataMember is TypeDefinition type))
                return;

            if (type.BaseType is null)
                return;
            
            var super = type.BaseType.Resolve();
            var superNode = context.GetOrCreateNode(super);
            
            Depend(node, superNode);
        }
    }
}