namespace Doktr.Core.Models.Constants;

public abstract class Constant : ICloneable
{
    public abstract void AcceptVisitor(IConstantVisitor visitor);

    public abstract Constant Clone();

    object ICloneable.Clone() => Clone();
}