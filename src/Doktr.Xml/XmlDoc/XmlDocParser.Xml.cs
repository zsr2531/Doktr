using static Doktr.Xml.XmlDoc.ThrowHelper;

namespace Doktr.Xml.XmlDoc;

public partial class XmlDocParser
{
    public bool IsEof => _nodes[_position].Kind == XmlNodeKind.EndOfFile;
    public XmlNode Lookahead => _nodes[_position];

    public XmlElementNode ExpectElement(params string[] names)
    {
        var node = Lookahead;
        if (node is not XmlElementNode element)
            return ThrowNodeTypeMismatch<XmlElementNode>(Lookahead, XmlNodeKind.Element);
        if (!names.Contains(element.Name))
            return ThrowNodeNameMismatch<XmlElementNode>(element.Span, names, element.Name);

        return Consume<XmlElementNode>();
    }

    public XmlEndElementNode ExpectEndElement(string name)
    {
        var node = Lookahead;
        if (node is not XmlEndElementNode element)
            return ThrowNodeTypeMismatch<XmlEndElementNode>(Lookahead, XmlNodeKind.Element);
        if (element.Name != name)
            return ThrowNodeNameMismatch<XmlEndElementNode>(element.Span, name, element.Name);

        return Consume<XmlEndElementNode>();
    }

    public XmlEmptyElementNode ExpectEmptyElement(params string[] names)
    {
        var node = Lookahead;
        if (node is not XmlEmptyElementNode element)
            return ThrowNodeTypeMismatch<XmlEmptyElementNode>(Lookahead, XmlNodeKind.EmptyElement);
        if (!names.Contains(element.Name))
            return ThrowNodeNameMismatch<XmlEmptyElementNode>(element.Span, names, element.Name);

        return Consume<XmlEmptyElementNode>();
    }

    public XmlTextNode ExpectText()
    {
        var node = Lookahead;
        if (node is not XmlTextNode)
            return ThrowNodeTypeMismatch<XmlTextNode>(Lookahead, XmlNodeKind.Text);

        return Consume<XmlTextNode>();
    }

    public XmlComplexNode ExpectElementOrEmptyElement(params string[] names)
    {
        return Lookahead switch
        {
            XmlElementNode { Name: { } s } element => names.Contains(s)
                ? Consume<XmlElementNode>()
                : ThrowNodeNameMismatch<XmlComplexNode>(element.Span, names, s),
            XmlEmptyElementNode { Name: { } s } emptyElement => names.Contains(s)
                ? Consume<XmlEmptyElementNode>()
                : ThrowNodeNameMismatch<XmlComplexNode>(emptyElement.Span, names, s),
            _ => ThrowNodeTypeMismatch<XmlComplexNode>(Consume(), XmlNodeKind.Element, XmlNodeKind.EmptyElement),
        };
    }

    public XmlNode Consume()
    {
        var node = _nodes[_position];
        if (!IsEof)
            _position++;

        return node;
    }

    private T Consume<T>()
        where T : XmlNode => (T) Consume();
}