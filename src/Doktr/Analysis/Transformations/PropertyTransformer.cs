using AsmResolver.DotNet;

namespace Doktr.Analysis.Transformations
{
    public class PropertyTransformer : ParentFixerTransformerBase
    {
        protected override int Visit(DependencyNode node, TransformationContext context)
        {
            if (!(node.MetadataMember is PropertyDefinition property))
                return 0;

            int delta = 0;
            var get = property.GetMethod;
            var set = property.SetMethod;
            
            if (get is not null)
                FixParent((DependencyNode) context.Mapping[get], node, ref delta);
            if (set is not null)
                FixParent((DependencyNode) context.Mapping[set], node, ref delta);

            return delta;
        }
    }
}