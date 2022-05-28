using static Doktr.Xml.XmlDoc.ThrowHelper;

namespace Doktr.Xml.XmlDoc;

public partial class XmlDocParser
{
    public bool IsEof => _position >= _nodes.Count;
    public XmlNode? Lookahead => IsEof ? null : _nodes[_position];

    public XmlElementNode ExpectElement(params string[] names)
    {
        var node = Lookahead;
        if (node is not XmlElementNode element)
            return ThrowNodeTypeMismatch<XmlElementNode>(Lookahead, XmlNodeKind.Element, LastNode);
        if (!names.Contains(element.Name))
            return ThrowNodeNameMismatch<XmlElementNode>(element.Span, names, element.Name);

        return Consume<XmlElementNode>();
    }

    public XmlEndElementNode ExpectEndElement(string name)
    {
        var node = Lookahead;
        if (node is not XmlEndElementNode element)
            return ThrowNodeTypeMismatch<XmlEndElementNode>(Lookahead, XmlNodeKind.Element, LastNode);
        if (element.Name != name)
            return ThrowNodeNameMismatch<XmlEndElementNode>(element.Span, name, element.Name);

        return Consume<XmlEndElementNode>();
    }

    public XmlEmptyElementNode ExpectEmptyElement(params string[] names)
    {
        var node = Lookahead;
        if (node is not XmlEmptyElementNode element)
            return ThrowNodeTypeMismatch<XmlEmptyElementNode>(Lookahead, XmlNodeKind.EmptyElement, LastNode);
        if (!names.Contains(element.Name))
            return ThrowNodeNameMismatch<XmlEmptyElementNode>(element.Span, names, element.Name);

        return Consume<XmlEmptyElementNode>();
    }

    public XmlTextNode ExpectText()
    {
        var node = Lookahead;
        if (node is not XmlTextNode)
            return ThrowNodeTypeMismatch<XmlTextNode>(Lookahead, XmlNodeKind.Text, LastNode);

        return Consume<XmlTextNode>();
    }

    public XmlNode ExpectElementOrEmptyElement(params string[] names)
    {
        return Lookahead switch
        {
            XmlElementNode { Name: { } s } element => names.Contains(s)
                ? Consume<XmlElementNode>()
                : ThrowNodeNameMismatch<XmlNode>(element.Span, names, s),
            XmlEmptyElementNode { Name: { } s } emptyElement => names.Contains(s)
                ? Consume<XmlEmptyElementNode>()
                : ThrowNodeNameMismatch<XmlNode>(emptyElement.Span, names, s),
            _ => throw new XmlDocParserException(
                $"Expected node type: {XmlNodeKind.Element} or {XmlNodeKind.EmptyElement}, got: {Lookahead?.Kind}",
                Consume().Span)
        };
    }

    public XmlNode Consume()
    {
        if (!IsEof)
            return _nodes[_position++];

        (_, _, int line, int col) = _nodes[^1].Span;
        var span = new TextSpan(line, col + 1, line, col + 1);

        throw new XmlDocParserException("Unexpected end of input", span);
    }

    private T Consume<T>()
        where T : XmlNode => (T) Consume();
}