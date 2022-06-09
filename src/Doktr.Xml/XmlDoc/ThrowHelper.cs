using System.Diagnostics.CodeAnalysis;

namespace Doktr.Xml.XmlDoc;

public static class ThrowHelper
{
    [DoesNotReturn]
    public static T ThrowNodeTypeMismatch<T>(XmlNode node, params XmlNodeKind[] expectedKinds)
    {
        string expected = string.Join(" or ", expectedKinds);
        throw new XmlDocParserException($"Expected node type: {expected}, got: {node.Kind}", node.Span);
    }

    [DoesNotReturn]
    public static T ThrowNodeNameMismatch<T>(TextSpan span, string expectedName, string actualName)
    {
        throw new XmlDocParserException($"Expected node with name: '{expectedName}', got '{actualName}'", span);
    }

    [DoesNotReturn]
    public static T ThrowNodeNameMismatch<T>(TextSpan span, string[] expectedNames, string actualName)
    {
        string names = string.Join("' or '", expectedNames);
        throw new XmlDocParserException($"Expected node with name: '{names}', got '{actualName}'", span);
    }
}