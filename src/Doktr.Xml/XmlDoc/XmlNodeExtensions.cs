namespace Doktr.Xml.XmlDoc;

public static class XmlNodeExtensions
{
    public static string ExpectAttribute(this XmlElementNode element, string key)
    {
        var attributes = element.Attributes;
        if (attributes.TryGetValue(key, out string? value))
            return value;

        throw new XmlDocParserException($"Expected an attribute with name '{key}'", element.Span);
    }

    public static string ExpectAttribute(this XmlEmptyElementNode emptyElement, string key)
    {
        var attributes = emptyElement.Attributes;
        if (attributes.TryGetValue(key, out string? value))
            return value;

        throw new XmlDocParserException($"Expected an attribute with name '{key}'", emptyElement.Span);
    }
}