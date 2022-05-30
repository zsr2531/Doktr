namespace Doktr.Xml.XmlDoc;

public readonly struct XmlDocDiagnostic
{
    public static XmlDocDiagnostic MakeWarning(TextSpan span, string message, Exception? exception = null) =>
        new(XmlDocDiagnosticSeverity.Warning, span, message, exception);

    public static XmlDocDiagnostic MakeError(TextSpan span, string message, Exception? exception = null) =>
        new(XmlDocDiagnosticSeverity.Error, span, message, exception);

    private XmlDocDiagnostic(
        XmlDocDiagnosticSeverity severity,
        TextSpan span,
        string message,
        Exception? exception = null)
    {
        Severity = severity;
        Span = span;
        Message = message;
        Exception = exception;
    }

    public XmlDocDiagnosticSeverity Severity { get; }
    public TextSpan Span { get; }
    public string Message { get; }
    public Exception? Exception { get; }

    public override string ToString() => $"({Span}): {Message}.";
}