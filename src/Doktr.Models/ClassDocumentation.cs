using System.Collections.Immutable;
using Doktr.Models.References;

namespace Doktr.Models;

public class ClassDocumentation : TypeDocumentation
{
    public ClassDocumentation(string assembly, string ns, string name, IReference baseType)
        : base(assembly, ns, name)
    {
        BaseType = baseType;
    }

    public ClassDocumentation(ClassDocumentation other)
        : base(other)
    {
        BaseType = other.BaseType;
        Inheritance = other.Inheritance;
        Implementations = other.Implementations;
    }
        
    public IReference BaseType
    {
        get;
        init;
    }
        
    public ImmutableArray<IReference> Inheritance
    {
        get;
        init;
    } = ImmutableArray<IReference>.Empty;
        
    public ImmutableArray<IReference> Implementations
    {
        get;
        init;
    } = ImmutableArray<IReference>.Empty;
}