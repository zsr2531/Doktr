using System.Collections.ObjectModel;
using Doktr.Core.Models.Signatures;

namespace Doktr.Core.Models.Collections;

public class TypeSignatureCollection : Collection<TypeSignature>, ICloneable
{
    public TypeSignatureCollection Clone()
    {
        var clone = new TypeSignatureCollection();
        foreach (var item in this)
            clone.Add(item.Clone());

        return clone;
    }
    
    object ICloneable.Clone() => Clone();
}