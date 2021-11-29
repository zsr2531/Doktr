using System.Collections.Immutable;
using Doktr.Models.References;

namespace Doktr.Models;

public class RecordDocumentation : TypeDocumentation
{
    public RecordDocumentation(
        string assembly,
        string ns,
        string name,
        IReference baseType,
        ImmutableArray<ParameterDocumentation> elements)
        : base(assembly, ns, name)
    {
        Elements = elements;
        BaseType = baseType;
    }

    public RecordDocumentation(RecordDocumentation other)
        : base(other)
    {
        BaseType = other.BaseType;
        Elements = other.Elements;
        Inheritance = other.Inheritance;
        Implementations = other.Implementations;
    }

    public IReference BaseType
    {
        get;
        init;
    }
        
    public ImmutableArray<ParameterDocumentation> Elements
    {
        get;
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