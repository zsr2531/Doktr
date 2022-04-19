using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models.Segments;

public enum TypeArgumentVarianceKind
{
    Invariant,
    Covariant,
    Contravariant
}

public class TypeParameterSegment
{
    public TypeParameterSegment(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public TypeArgumentVarianceKind Variance { get; set; } = TypeArgumentVarianceKind.Invariant;
    public TypeArgumentConstraintCollection Constraints { get; set; } = new();
    public DocumentationFragmentCollection Documentation { get; set; } = new();
}