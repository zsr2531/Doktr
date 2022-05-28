using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class MethodCodeReferenceCollection : Collection<CodeReference>, ICloneable
{
    protected override void InsertItem(int index, CodeReference item)
    {
        AssertCorrectCodeReferenceKind(item);
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, CodeReference item)
    {
        AssertCorrectCodeReferenceKind(item);
        base.SetItem(index, item);
    }

    public MethodCodeReferenceCollection Clone()
    {
        var clone = new MethodCodeReferenceCollection();
        foreach (var item in this)
            clone.Add(item);

        return clone;
    }

    object ICloneable.Clone() => Clone();

    private static void AssertCorrectCodeReferenceKind(CodeReference codeReference)
    {
        if (!codeReference.IsMethod)
        {
            throw new InvalidOperationException("Only method references are allowed in this collection.");
        }
    }
}