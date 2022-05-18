using Doktr.Lifters.Common.XmlDoc.Collections;

namespace Doktr.Lifters.Common.XmlDoc;

public abstract class XmlDocToken
{
    protected XmlDocToken(TextSpan span)
    {
        Span = span;
    }

    public TextSpan Span { get; }
}

public class XmlDocOpenTag : XmlDocToken
{
    public static XmlDocOpenTag MakeOpenTag(TextSpan span, string tagName, XmlAttributeMap attributes)
    {
        return new XmlDocOpenTag(span, false, tagName, attributes);
    }

    public static XmlDocOpenTag MakeEmptyElement(TextSpan span, string tagName, XmlAttributeMap attributes)
    {
        return new XmlDocOpenTag(span, true, tagName, attributes);
    }

    private XmlDocOpenTag(TextSpan span, bool isEmptyElement, string tagName, XmlAttributeMap attributes)
        : base(span)
    {
        IsEmptyElement = isEmptyElement;
        TagName = tagName;
        Attributes = attributes;
    }

    public bool IsEmptyElement { get; }
    public string TagName { get; }
    public XmlAttributeMap Attributes { get; }
}

public class XmlDocCloseTag : XmlDocToken
{
    public XmlDocCloseTag(TextSpan span, string tagName)
        : base(span)
    {
        TagName = tagName;
    }

    public string TagName { get; }
}

public class XmlDocText : XmlDocToken
{
    public XmlDocText(TextSpan span, string text)
        : base(span)
    {
        Text = text;
    }

    // TODO: Should this return formatted text? Everything is indented by 8+ spaces and code is a mess...
    // We are going to have to deal with it sooner than later so might as well do it right away here.
    // OR: We could just return the raw text and let the backends/renderers deal with it.
    public string Text { get; }
}