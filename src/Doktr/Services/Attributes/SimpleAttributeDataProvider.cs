namespace Doktr.Services.Attributes;

public class SimpleAttributeDataProvider<T> : IAttributeDataProvider<T>
{
    private readonly T _value;

    public SimpleAttributeDataProvider(T value)
    {
        _value = value;
    }

    public T Next() => _value;
}