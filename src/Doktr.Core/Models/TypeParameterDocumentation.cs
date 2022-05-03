using Doktr.Core.Models.Collections;

namespace Doktr.Core.Models;

public enum TypeParameterVarianceKind
{
    Invariant,
    Covariant,
    Contravariant
}

public class TypeParameterDocumentation : ICloneable
{
    public TypeParameterDocumentation(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public TypeParameterVarianceKind Variance { get; set; } = TypeParameterVarianceKind.Invariant;
    public TypeParameterConstraintCollection Constraints { get; set; } = new();
    public DocumentationFragmentCollection Documentation { get; set; } = new();

    public TypeParameterDocumentation Clone() => new(Name)
    {
        Variance = Variance,
        Constraints = Constraints.Clone(),
        Documentation = Documentation.Clone()
    };

    object ICloneable.Clone() => Clone();
}