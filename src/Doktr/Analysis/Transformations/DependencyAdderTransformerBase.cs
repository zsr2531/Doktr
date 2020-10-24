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

        protected abstract void Visit(DependencyNodeBase node, TransformationContext context);

        protected static void Depend(DependencyNodeBase from, DependencyNodeBase to) => from.Dependencies.Add(to);
    }
}