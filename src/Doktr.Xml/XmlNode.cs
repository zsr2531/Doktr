using System.Diagnostics.CodeAnalysis;
using Doktr.Xml.Collections;

namespace Doktr.Xml;

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

public abstract class XmlComplexNode : XmlNode
{
    protected XmlComplexNode(TextSpan span)
        : base(span)
    {
    }

    public abstract string Name { get; }
    public abstract XmlAttributeMap Attributes { get; }

    public bool TryGetAttribute(string key, [NotNullWhen(true)] out string? value) =>
        Attributes.TryGetValue(key, out value);
}

public class XmlElementNode : XmlComplexNode
{
    public XmlElementNode(TextSpan span, string name)
        : base(span)
    {
        Name = name;
    }

    public override string Name { get; }
    public override XmlAttributeMap Attributes { get; } = new();
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

public class XmlEmptyElementNode : XmlComplexNode
{
    public XmlEmptyElementNode(TextSpan span, string name)
        : base(span)
    {
        Name = name;
    }

    public override string Name { get; }
    public override XmlAttributeMap Attributes { get; } = new();
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

public class XmlEndOfFileNode : XmlNode
{
    public XmlEndOfFileNode(TextSpan span)
        : base(span)
    {
    }

    public override XmlNodeKind Kind => XmlNodeKind.EndOfFile;
}