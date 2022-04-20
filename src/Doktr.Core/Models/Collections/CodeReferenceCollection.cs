using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class CodeReferenceCollection : Collection<CodeReference>, ICloneable
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

    public CodeReferenceCollection Clone()
    {
        var clone = new CodeReferenceCollection();
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