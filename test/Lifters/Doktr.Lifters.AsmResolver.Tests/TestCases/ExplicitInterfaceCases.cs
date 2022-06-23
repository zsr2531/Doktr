namespace Doktr.Lifters.AsmResolver.Tests.TestCases;

public partial interface IInterface
{
    void Explicit();
}

public partial class ClassWithInterface
{
    void IInterface.Explicit()
    {
    }
}