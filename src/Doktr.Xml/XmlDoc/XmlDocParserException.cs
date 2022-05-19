namespace Doktr.Xml.XmlDoc;

public class XmlDocParserException : Exception
{
    public XmlDocParserException(string message, TextSpan span)
    {
        Message = message + $" at ({span})";
        Span = span;
    }

    public XmlDocParserException(string message, int line, int column)
    {
        Message = message + $" at ({line}:{column})";
        Span = new TextSpan(line, column);
    }

    public override string Message { get; }
    public TextSpan Span { get; }
}