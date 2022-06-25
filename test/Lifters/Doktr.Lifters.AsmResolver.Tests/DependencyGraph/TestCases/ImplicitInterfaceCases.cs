namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases;

public partial interface IInterface
{
    void ImplicitMethod();

    void ImplicitGenericMethodWithParam<T>(T param);
}

public interface IGenericInterface<out T>
{
    T ImplicitGenericMethod();
}

public partial class ClassWithInterface : IGenericInterface<int>
{
    public void ImplicitMethod()
    {
    }

    public void ImplicitGenericMethodWithParam<T>(T param)
    {
    }

    public int ImplicitGenericMethod() => 0;
}