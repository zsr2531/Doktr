namespace Doktr.Xml.XmlDoc;

public readonly struct XmlDocDiagnostic
{
    public XmlDocDiagnostic(XmlDocDiagnosticSeverity severity, string message, Exception? exception = null)
    {
        Severity = severity;
        Span = null;
        Message = message;
        Exception = exception;
    }

    public XmlDocDiagnostic(
        XmlDocDiagnosticSeverity severity,
        TextSpan? span,
        string message,
        Exception? exception = null)
    {
        Severity = severity;
        Span = span;
        Message = message;
        Exception = exception;
    }

    public XmlDocDiagnosticSeverity Severity { get; }
    public TextSpan? Span { get; }
    public string Message { get; }
    public Exception? Exception { get; }

    public override string ToString() => $"({Span}): {Message}.";
}