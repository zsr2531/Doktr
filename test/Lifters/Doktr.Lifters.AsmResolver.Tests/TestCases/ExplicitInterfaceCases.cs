using System;

namespace Doktr.Lifters.AsmResolver.Tests.TestCases;

public partial interface IInterface
{
    event Action? ExplicitEvent;

    string ExplicitProperty { get; }

    void ExplicitMethod();
}

public partial class ClassWithInterface
{
    private event Action? _backing;

    event Action? IInterface.ExplicitEvent
    {
        add => _backing += value;
        remove => _backing -= value;
    }

    string IInterface.ExplicitProperty => "";

    void IInterface.ExplicitMethod()
    {
    }
}