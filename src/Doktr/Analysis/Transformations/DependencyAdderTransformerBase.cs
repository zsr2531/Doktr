namespace Doktr.Analysis.Transformations
{
    public abstract class DependencyAdderTransformerBase : ITransformer
    {
        public void Transform(DependencyNode node, TransformationContext context)
        {
            foreach (var child in node.Children)
                Transform(child, context);
            
            Visit(node, context);
        }

        protected abstract void Visit(DependencyNode node, TransformationContext context);

        protected static void Depend(DependencyNode from, DependencyNode to) => from.Dependencies.Add(to);
    }
}