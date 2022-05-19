using Antlr4.Runtime;
using Doktr.Xml.Collections;

namespace Doktr.Xml;

public class XmlParser : IXmlParser, IAntlrErrorListener<IToken>, IAntlrErrorListener<int>
{
    private readonly TextReader _reader;

    public XmlParser(string s)
        : this(new StringReader(s))
    {
    }

    public XmlParser(TextReader reader)
    {
        _reader = reader;
    }

    public bool HasFatalErrors => Diagnostics.HasFatal;
    public bool HasErrors => !Diagnostics.IsEmpty;
    public XmlDiagnosticCollection Diagnostics { get; } = new();

    public XmlNodeCollection ParseXmlNodes()
    {
        var unit = ParseInput();
        return CollectNodes(unit);
    }

    private Xml.UnitContext ParseInput()
    {
        var lexer = new XmlLexer(new AntlrInputStream(_reader));
        var parser = new Xml(new CommonTokenStream(lexer));

        lexer.AddErrorListener(this);
        parser.AddErrorListener(this);

        return parser.unit();
    }

    void IAntlrErrorListener<IToken>.SyntaxError(
        TextWriter output,
        IRecognizer recognizer,
        IToken offendingSymbol,
        int line,
        int charPositionInLine,
        string msg,
        RecognitionException e) => HandleError(line, charPositionInLine, msg, e);

    void IAntlrErrorListener<int>.SyntaxError(
        TextWriter output,
        IRecognizer recognizer,
        int offendingSymbol,
        int line,
        int charPositionInLine,
        string msg,
        RecognitionException e) => HandleError(line, charPositionInLine, msg, e);

    private void HandleError(int line, int col, string message, Exception? ex)
    {
        var span = new TextSpan(line, col);
        var diagnostic = new XmlDiagnostic(span, message, ex);
        Diagnostics.Add(diagnostic);
    }

    private static XmlNodeCollection CollectNodes(Xml.UnitContext unit)
    {
        var nodes = new XmlNodeCollection();
        foreach (var node in unit.node())
        {
            try
            {
                var transformed = node.Accept(XmlNodeConstructor.Instance);
                nodes.Add(transformed);
            }
            catch (Exception ex)
            {
                // TODO: Something went wrong here, maybe log it?
            }
        }

        return nodes;
    }
}