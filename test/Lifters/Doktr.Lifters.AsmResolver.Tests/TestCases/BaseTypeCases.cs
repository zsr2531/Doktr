namespace Doktr.Lifters.AsmResolver.Tests.TestCases;

public abstract partial class AbstractClass
{
}

public partial class SubClass : AbstractClass
{
}

public partial interface IInterface
{
}

public partial class ClassWithInterface : IInterface
{
}

public partial class SubClassWithInterface : AbstractClass, IInterface
{
}