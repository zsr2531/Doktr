using System;

namespace Doktr.Services.Attributes;

public class ComplexAttributeDataProvider<T> : IAttributeDataProvider<T>
{
    private readonly T[] _values;
    private int _index;

    public ComplexAttributeDataProvider(T[] values)
    {
        _values = values;
        _index = 0;
    }

    public T Next()
    {
        if (_index >= _values.Length)
            throw new InvalidOperationException("Reached the end of attribute data.");

        return _values[_index++];
    }
}