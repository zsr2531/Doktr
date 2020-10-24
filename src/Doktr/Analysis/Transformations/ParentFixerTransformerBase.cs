using System;

namespace Doktr.Analysis.Transformations
{
    public abstract class ParentFixerTransformerBase : ITransformer
    {
        public void Transform(DependencyNode node, TransformationContext context) => Iterate(node, context);

        protected abstract int Visit(DependencyNode node, TransformationContext context);

        private int Iterate(DependencyNode node, TransformationContext context)
        {
            for (int i = 0; i < node.Children.Count; i = Math.Max(0, ++i))
                i += Iterate(node.Children[i], context);   

            return Visit(node, context);
        }

        protected static void FixParent(DependencyNode node, DependencyNode newParent, ref int delta)
        {
            if (ReferenceEquals(newParent, node.Owner))
                return;

            node.Owner!.Children.Remove(node);
            newParent.Children.Add(node);
            delta--;
        }
    }
}