// ReSharper disable InconsistentNaming
namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases;

public abstract partial class AbstractClass
{
    public abstract void AbstractMethod();

    public virtual void VirtualMethod<T, U>(T arg1, U arg2)
    {
    }
}

public partial class SubClass
{
    public override void AbstractMethod()
    {
    }
}

public class SubSubClass : SubClass
{
    public override void VirtualMethod<T, U>(T arg1, U arg2)
    {
    }
}