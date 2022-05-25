using Antlr4.Runtime;

namespace Doktr.Xml;

public class XmlNodeConstructor : XmlBaseVisitor<XmlNode>
{
    public static readonly XmlNodeConstructor Instance = new();

    private XmlNodeConstructor()
    {
    }

    public override XmlNode VisitElement(Xml.ElementContext context)
    {
        var span = CreateTextSpan(context);
        string name = context.IDENTIFIER().GetText();
        var node = new XmlElementNode(span, name);
        var rawAttributes = context.attribute();
        AddAttributes(node, rawAttributes);

        return node;
    }

    public override XmlNode VisitEndElement(Xml.EndElementContext context)
    {
        var span = CreateTextSpan(context);
        string name = context.IDENTIFIER().GetText();

        return new XmlEndElementNode(span, name);
    }

    public override XmlNode VisitEmptyElement(Xml.EmptyElementContext context)
    {
        var span = CreateTextSpan(context);
        string name = context.IDENTIFIER().GetText();
        var node = new XmlEmptyElementNode(span, name);
        var rawAttributes = context.attribute();
        AddAttributes(node, rawAttributes);

        return node;
    }

    public override XmlNode VisitTextElement(Xml.TextElementContext context)
    {
        var span = CreateTextSpan(context);
        string text = context.TEXT().GetText();

        // If we only have whitespace, don't bother creating a node.
        return text.All(char.IsWhiteSpace)
            ? null!
            : new XmlTextNode(span, text);
    }

    private static TextSpan CreateTextSpan(ParserRuleContext context)
    {
        var start = context.Start;
        var stop = context.Stop;

        return new TextSpan(start.Line, start.Column, stop.Line, stop.Column);
    }

    private static void AddAttributes(IHasAttributes node, Xml.AttributeContext[] attributes)
    {
        foreach (var attribute in attributes)
        {
            string key = attribute.IDENTIFIER().GetText();
            string value = attribute.@string().Accept(StringVisitor.Instance);
            node.Attributes[key] = value;
        }
    }

    private class StringVisitor : XmlBaseVisitor<string>
    {
        // ReSharper disable once MemberHidesStaticFromOuterClass
        internal static readonly StringVisitor Instance = new();

        private StringVisitor()
        {
        }

        public override string VisitString(Xml.StringContext context) => context.GetText()[1..^1];
    }
}