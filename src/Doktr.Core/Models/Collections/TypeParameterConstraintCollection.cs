using System.Collections.ObjectModel;
using Doktr.Core.Models.Constraints;

namespace Doktr.Core.Models.Collections;

public class TypeParameterConstraintCollection : Collection<TypeParameterConstraint>, ICloneable
{
    public void AssertAtMostOneTypeKindConstraint()
    {
        for (int i = 1; i < Count; i++)
        {
            if (this[i] is TypeKindTypeParameterConstraint)
            {
                throw new InvalidOperationException(
                    "Only one type kind constraint is allowed and only as the first constraint.");
            }
        }
    }
    
    public TypeParameterConstraintCollection Clone()
    {
        var clone = new TypeParameterConstraintCollection();
        foreach (var constraint in this)
            clone.Add(constraint.Clone());

        return clone;
    }
    
    object ICloneable.Clone() => Clone();
}