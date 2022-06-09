namespace Doktr.Xml.XmlDoc;

public static class XmlNodeExtensions
{
    public static bool IsNotEndElementOrEof(this XmlNode node) =>
        node.Kind is not XmlNodeKind.EndElement and not XmlNodeKind.EndOfFile;

    public static string ExpectAttribute(this XmlComplexNode node, string key)
    {
        if (node.TryGetAttribute(key, out string? value))
            return value;

        throw new XmlDocParserException($"Expected a '{key}' attribute", node.Span);
    }
}