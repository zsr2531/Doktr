using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class TypeArgumentConstraintCollection : Collection<TypeParameterConstraint>, ICloneable
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
    
    public TypeArgumentConstraintCollection Clone()
    {
        var clone = new TypeArgumentConstraintCollection();
        foreach (var constraint in this)
            clone.Add(constraint.Clone());

        return clone;
    }
    
    object ICloneable.Clone() => Clone();
}