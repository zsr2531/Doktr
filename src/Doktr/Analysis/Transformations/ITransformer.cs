namespace Doktr.Analysis.Transformations
{
    public interface ITransformer
    {
        void Transform(DependencyNode node, TransformationContext context);
    }
}