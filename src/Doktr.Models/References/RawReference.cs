using System.Collections.Immutable;

namespace Doktr.Models.References;

public class RawReference : IReference
{
    public RawReference(string cref, string text)
    {
        Cref = cref;
        Text = text;
    }

    public string Cref
    {
        get;
    }

    public string Text
    {
        get;
    }

    public ImmutableArray<IReference> GenericParameters
    {
        get;
        init;
    } = ImmutableArray<IReference>.Empty;

    public override string ToString() => Text;
}