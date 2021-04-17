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

            if (add is not null && context.Mapping.TryGetValue(add, out var adder))
                FixParent(adder, node, ref delta);
            if (remove is not null && context.Mapping.TryGetValue(remove, out var remover))
                FixParent(remover, node, ref delta);
            if (fire is not null && context.Mapping.TryGetValue(fire, out var firer))
                FixParent(firer, node, ref delta);

            return delta;
        }
    }
}