using Doktr.Lifters.Common.XmlDoc.Collections;

namespace Doktr.Lifters.Common.XmlDoc.Xml;

public abstract class XmlNode
{
    protected XmlNode(TextSpan span)
    {
        Span = span;
    }

    public TextSpan Span { get; }
    public abstract XmlNodeKind Kind { get; }

    public override string ToString() => $"<{Kind}>({Span})";
}

public interface IHasAttributes
{
    XmlAttributeMap Attributes { get; }
}

public class XmlElementNode : XmlNode, IHasAttributes
{
    public XmlElementNode(TextSpan span, string name)
        : base(span)
    {
        Name = name;
    }

    public string Name { get; }
    public XmlAttributeMap Attributes { get; } = new();
    public override XmlNodeKind Kind => XmlNodeKind.Element;
}

public class XmlEndElementNode : XmlNode
{
    public XmlEndElementNode(TextSpan span, string name)
        : base(span)
    {
        Name = name;
    }

    public string Name { get; }
    public override XmlNodeKind Kind => XmlNodeKind.EndElement;
}

public class XmlEmptyElementNode : XmlNode, IHasAttributes
{
    public XmlEmptyElementNode(TextSpan span, string name)
        : base(span)
    {
        Name = name;
    }

    public string Name { get; }
    public XmlAttributeMap Attributes { get; } = new();
    public override XmlNodeKind Kind => XmlNodeKind.EmptyElement;
}

public class XmlTextNode : XmlNode
{
    public XmlTextNode(TextSpan span, string text)
        : base(span)
    {
        Text = text;
    }

    public string Text { get; }
    public override XmlNodeKind Kind => XmlNodeKind.Text;
}