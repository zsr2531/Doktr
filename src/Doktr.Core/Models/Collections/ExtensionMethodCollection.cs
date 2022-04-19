using System.Collections.ObjectModel;

namespace Doktr.Core.Models.Collections;

public class ExtensionMethodCollection : Collection<CodeReference>
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

    private static void AssertCorrectCodeReferenceKind(CodeReference codeReference)
    {
        if (!codeReference.IsMethod)
        {
            throw new InvalidOperationException("Only method references are allowed in this collection.");
        }
    }
}