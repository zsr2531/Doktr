using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class VersionCollection : Collection<string>, ICloneable
{
    public VersionCollection Clone()
    {
        var clone = new VersionCollection();
        foreach (string version in this)
            clone.Add(version);
        
        return clone;
    }
    
    object ICloneable.Clone() => Clone();
}