namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases;

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

// ReSharper disable once RedundantExtendsListEntry
public partial class SubClassWithInterface : ClassWithInterface, IInterface
{
}