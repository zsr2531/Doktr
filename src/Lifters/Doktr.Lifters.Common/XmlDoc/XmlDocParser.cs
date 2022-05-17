using System.Text;
using Doktr.Lifters.Common.XmlDoc.Collections;

namespace Doktr.Lifters.Common.XmlDoc;

public class XmlDocParser
{
    private readonly TextReader _reader;
    private char? _lookahead;

    public XmlDocParser(TextReader reader)
    {
        _reader = reader;
    }

    public char Lookahead => _lookahead ??= NextChar();
    public int Line { get; private set; } = 1;
    public int Column { get; private set; } = 1;

    public RawXmlDocEntryMap ParseXmlDoc()
    {
        var map = new RawXmlDocEntryMap();
        ParseProlog();
        ParseOpenTag("doc");
        SkipPrologue();
        ParseOpenTag("members");

        ParseCloseTag("members");
        ParseCloseTag("doc");
        return map;

        void SkipPrologue()
        {
            ParseOpenTag("assembly");
            ParseOpenTag("name");
            var assembly = ParseText();
            ParseCloseTag("name");
            ParseCloseTag("assembly");
        }
    }

    public XmlDocToken ParseProlog()
    {
        int line = Line, col = Column;
        ExpectChar('<');
        ExpectChar('?');

        SkipWhitespace();
        string tagName = ParseIdentifier();
        if (tagName != "xml")
            throw new XmlDocParserException($"Expected prolog tag (<?xml ... ?>), got '<?{tagName} ...'", line, col);

        var attributes = ParseAttributes();
        ExpectChar('?');
        ExpectChar('>');

        return XmlDocOpenTag.MakeEmptyElement(MakeTextSpan(line, col), "xml", attributes);
    }

    public XmlDocToken ParseOpenTag(string? expected = null)
    {
        int line = Line, col = Column;
        SkipWhitespace();
        ExpectChar('<');

        SkipWhitespace();
        string tagName = ParseIdentifier();
        if (tagName != expected)
        {
            throw new XmlDocParserException($"Expected opening tag '{expected}' but found '{tagName}'",
                MakeTextSpan(line, col));
        }

        var attributes = ParseAttributes();

        if (Lookahead == '>')
        {
            NextChar(); // Consume the '>'
            return XmlDocOpenTag.MakeOpenTag(MakeTextSpan(line, col), tagName, attributes);
        }

        // Consume the '/' and the '>'
        NextChar();
        ExpectChar('>');
        return XmlDocOpenTag.MakeEmptyElement(MakeTextSpan(line, col), tagName, attributes);
    }

    public XmlDocCloseTag ParseCloseTag(string expectedTagName)
    {
        int line = Line, col = Column;
        SkipWhitespace();
        ExpectChar('<');
        SkipWhitespace();
        ExpectChar('/');
        SkipWhitespace();

        string tagName = ParseIdentifier();
        if (tagName != expectedTagName)
        {
            throw new XmlDocParserException($"Expected closing tag '{expectedTagName}' but found '{tagName}'",
                MakeTextSpan(line, col));
        }

        SkipWhitespace();
        ExpectChar('>');

        return new XmlDocCloseTag(MakeTextSpan(line, col), tagName);
    }

    public XmlDocText ParseText()
    {
        int line = Line, col = Column;
        var sb = new StringBuilder();
        while (Lookahead != '<')
            sb.Append(NextNonEofChar());

        return new XmlDocText(MakeTextSpan(line, col), sb.ToString());
    }

    public string ParseString()
    {
        char quote = ExpectChar('\'', '"');
        var sb = new StringBuilder();
        while (Lookahead != quote)
            sb.Append(NextNonEofChar());

        ExpectChar(quote);
        return sb.ToString();
    }

    public string ParseIdentifier()
    {
        var sb = new StringBuilder();
        while (!char.IsWhiteSpace(Lookahead) && Lookahead is not '<' and not '>' and not '/')
        {
            sb.Append(NextChar());
        }

        return sb.ToString();
    }

    public void SkipWhitespace()
    {
        while (char.IsWhiteSpace(Lookahead))
            NextNonEofChar();
    }

    public char NextNonEofChar()
    {
        char ch = NextChar();
        if (ch == '\0')
            throw new XmlDocParserException("Unexpected end of file", Line, Column + 1);

        return ch;
    }

    public char ExpectChar(char expected)
    {
        char ch = NextNonEofChar();
        if (ch != expected)
            throw new XmlDocParserException($"Expected '{expected}', got '{ch}'", Line, Column + 1);

        return ch;
    }

    public char ExpectChar(char expected1, char expected2)
    {
        char ch = NextNonEofChar();
        if (ch != expected1 && ch != expected2)
        {
            throw new XmlDocParserException(
                $"Expected '{expected1}' or '{expected2}', got '{ch}'", Line, Column - 1);
        }

        return ch;
    }

    public TextSpan MakeTextSpan(int startLine, int startColumn) => new(startLine, startColumn, Line, Column);

    private XmlDocTokenAttributeMap ParseAttributes()
    {
        SkipWhitespace();
        var attributes = new XmlDocTokenAttributeMap();
        while (Lookahead is not '>' and not '/')
        {
            string name = ParseIdentifier();

            SkipWhitespace();
            ExpectChar('=');
            SkipWhitespace();

            string value = ParseString();
            SkipWhitespace();

            // TODO: If the user already defined an attribute with this name, we'll overwrite it. Should this warn?
            attributes[name] = value;
        }

        return attributes;
    }

    private char NextChar()
    {
        bool hasLookahead = _lookahead.HasValue;
        int ch = hasLookahead ? Lookahead : _reader.Read();
        if (hasLookahead)
            _lookahead = null;

        switch (ch)
        {
            case -1:
                return '\0';

            case '\n':
                Line++;
                Column = 1;
                break;

            default:
                Column++;
                break;
        }

        return (char) ch;
    }
}