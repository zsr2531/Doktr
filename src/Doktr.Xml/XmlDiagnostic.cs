namespace Doktr.Xml;

public readonly struct XmlDiagnostic
{
    public XmlDiagnostic(TextSpan span, string message, Exception? exception = null)
    {
        Span = span;
        Message = message;
        Exception = exception;
    }

    public TextSpan Span { get; }
    public string Message { get; }
    public Exception? Exception { get; }

    public override string ToString() => $"({Span}): {Message}.";
}