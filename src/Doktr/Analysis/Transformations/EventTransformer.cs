using AsmResolver.DotNet;

namespace Doktr.Analysis.Transformations
{
    public class EventTransformer : ParentFixerTransformerBase
    {
        protected override int Visit(DependencyNode node, TransformationContext context)
        {
            if (!(node.MetadataMember is EventDefinition @event))
                return 0;

            int delta = 0;
            var add = @event.AddMethod;
            var remove = @event.RemoveMethod;
            var fire = @event.FireMethod;

            if (add is not null)
                FixParent((DependencyNode) context.Mapping[add], node, ref delta);
            if (remove is not null)
                FixParent((DependencyNode) context.Mapping[remove], node, ref delta);
            if (fire is not null)
                FixParent((DependencyNode) context.Mapping[fire], node, ref delta);

            return delta;
        }
    }
}