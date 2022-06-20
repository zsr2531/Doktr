namespace Doktr.Lifters.Common.DependencyGraph;

public enum DependencyEdgeKind
{
    Extension, // class extends another class
    Override, // method overrides a method of a superclass
    Implementation, // class/method implements a(n) (method of an) interface
    ExplicitImplementation, // method explicitly implements a method of an interface
    OtherConstructor // constructor calls other constructor
}