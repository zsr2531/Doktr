using System;

namespace Doktr.Lifters.AsmResolver.Tests.DependencyGraph.TestCases;

public partial interface IInterface
{
    event Action? ExplicitEvent;

    string ExplicitProperty { get; }

    void ExplicitMethod();
}

public partial class ClassWithInterface
{
#pragma warning disable CS0067
    private event Action? Backing;
#pragma warning restore CS0067

    event Action? IInterface.ExplicitEvent
    {
        add => Backing += value;
        remove => Backing -= value;
    }

    string IInterface.ExplicitProperty => "";

    void IInterface.ExplicitMethod()
    {
    }
}