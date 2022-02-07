using System;

namespace Doktr.Services.XmlDocParser;

public class MalformedXmlDocException : Exception
{
    public MalformedXmlDocException(string message)
        : base(message)
    {
    }

    public MalformedXmlDocException(string got, params string[] expected)
        : base($"Expected one of: {{ '{string.Join("', '", expected)}' }}, but instead got: '{got}'")
    {
    }
}