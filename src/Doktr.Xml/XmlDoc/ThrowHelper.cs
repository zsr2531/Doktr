using System.Diagnostics.CodeAnalysis;

namespace Doktr.Xml.XmlDoc;

public static class ThrowHelper
{
    private const string Nothing = "nothing";

    [DoesNotReturn]
    public static T ThrowNodeTypeMismatch<T>(XmlNode? node, XmlNodeKind expected, XmlNode lastNode)
    {
        (_, _, int line, int col) = lastNode.Span;
        var span = node?.Span ?? new TextSpan(line, col + 1, line, col + 1);
        string got = node?.Kind.ToString() ?? Nothing;

        throw new XmlDocParserException($"Expected node type: {expected}, got: {got}", span);
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