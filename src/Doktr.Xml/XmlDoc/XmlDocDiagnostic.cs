namespace Doktr.Xml.XmlDoc;

public readonly struct XmlDocDiagnostic
{
    public static XmlDocDiagnostic MakeWarning(TextSpan span, string message) =>
        new(XmlDocDiagnosticSeverity.Warning, span, message);

    public static XmlDocDiagnostic MakeError(TextSpan span, string message) =>
        new(XmlDocDiagnosticSeverity.Error, span, message);

    private XmlDocDiagnostic(
        XmlDocDiagnosticSeverity severity,
        TextSpan span,
        string message)
    {
        Severity = severity;
        Span = span;
        Message = message;
    }

    public XmlDocDiagnosticSeverity Severity { get; }
    public TextSpan Span { get; }
    public string Message { get; }

    public override string ToString() => $"{Message} at ({Span}).";
}