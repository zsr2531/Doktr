using System;
using System.Collections.ObjectModel;

namespace Doktr.Dependencies;

public class OwnerCollection : Collection<DependencyNode>
{
    private readonly DependencyNode _owner;

    public OwnerCollection(DependencyNode owner)
    {
        _owner = owner;
    }

    protected override void ClearItems()
    {
        foreach (var child in this)
            child.Parent = null;

        base.ClearItems();
    }

    protected override void InsertItem(int index, DependencyNode item)
    {
        AssertNoParent(item);
        item.Parent = _owner;
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        this[index].Parent = null;
        base.RemoveItem(index);
    }

    protected override void SetItem(int index, DependencyNode item)
    {
        AssertNoParent(item);
        item.Parent = _owner;
        base.SetItem(index, item);
    }

    private void AssertNoParent(DependencyNode node)
    {
        if (node.Parent is not null && node.Parent != _owner)
            throw new InvalidOperationException("Node already has a parent.");
    }
}