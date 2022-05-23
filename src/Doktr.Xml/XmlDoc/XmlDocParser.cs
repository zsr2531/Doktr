using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models;
using Doktr.Core.Models.Fragments;
using Doktr.Xml.Collections;
using Doktr.Xml.XmlDoc.Collections;
using Doktr.Xml.XmlDoc.FragmentParsers;
using Doktr.Xml.XmlDoc.SectionParsers;

namespace Doktr.Xml.XmlDoc;

public class XmlDocParser : IXmlDocParser
{
    private readonly Dictionary<string, ISectionParser> _sectionParsers;
    private readonly Dictionary<string, IFragmentParser> _fragmentParsers;
    private readonly XmlNodeCollection _nodes;
    private int _position;

    public XmlDocParser(
        IEnumerable<ISectionParser> sectionParsers,
        IEnumerable<IFragmentParser> fragmentParsers,
        XmlNodeCollection nodes)
    {
        _sectionParsers = sectionParsers.ToDictionary(k => k.Tag, v => v);
        _fragmentParsers = fragmentParsers
                           .SelectMany(p => p.SupportedTags.Select(t => (p, t)))
                           .ToDictionary(k => k.t, v => v.p);
        _nodes = nodes;
    }

    public bool IsEof => _position >= _nodes.Count;
    public XmlNode? Lookahead => IsEof ? null : _nodes[_position];

    public RawXmlDocEntryMap ParseXmlDoc()
    {
        var map = new RawXmlDocEntryMap();
        bool hasPrologue = ParsePrologue();

        while (Lookahead is not XmlEndElementNode and not null)
        {
            var member = ExpectElement("member");
            string rawDocId = member.Attributes["name"];
            var docId = new CodeReference(rawDocId);
            var entry = new RawXmlDocEntry(docId);

            while (Lookahead is not XmlEndElementNode)
            {
                // Short circuit to summary if we encounter 'dangling' text.
                if (Lookahead is XmlTextNode)
                {
                    var text = ExpectText();
                    entry.Summary.Add(new TextFragment(text.Text));
                    continue;
                }

                ParseSection(entry);
            }

            map[docId] = entry;
            ExpectEndElement("member");
        }

        if (hasPrologue)
            ParseEpilogue();

        return map;

        bool ParsePrologue()
        {
            if (Lookahead is not XmlElementNode { Name: "doc" })
                return false;

            ExpectElement("doc");
            ExpectElement("assembly");
            ExpectElement("name");
            ExpectText();
            ExpectEndElement("name");
            ExpectEndElement("assembly");
            ExpectElement("members");
            return true;
        }

        void ParseEpilogue()
        {
            ExpectEndElement("members");
            ExpectEndElement("doc");
        }
    }

    public DocumentationFragment NextFragment()
    {
        return Lookahead switch
        {
            XmlTextNode => new TextFragment(((XmlTextNode) Consume()).Text),
            XmlElementNode { Name: { } s } => GetFragmentParser(s).ParseFragment(this),
            XmlEmptyElementNode { Name: { } s } => GetFragmentParser(s).ParseFragment(this),
            _ => throw new XmlDocParserException($"Unexpected node: {Lookahead}", Consume().Span)
        };

        IFragmentParser GetFragmentParser(string tag)
        {
            if (_fragmentParsers.TryGetValue(tag, out var fragmentParser))
                return fragmentParser;

            throw new XmlDocParserException($"Unsupported tag: {tag}", Consume().Span);
        }
    }

    public XmlElementNode ExpectElement(params string[] names)
    {
        var node = Lookahead;
        if (node is not XmlElementNode element)
            return ThrowNodeTypeMismatch<XmlElementNode>(Lookahead, XmlNodeKind.Element);
        if (!names.Contains(element.Name))
            return ThrowNodeNameMismatch<XmlElementNode>(element.Span, names, element.Name);

        return (XmlElementNode) Consume();
    }

    public XmlEndElementNode ExpectEndElement(string name)
    {
        var node = Lookahead;
        if (node is not XmlEndElementNode element)
            return ThrowNodeTypeMismatch<XmlEndElementNode>(Lookahead, XmlNodeKind.Element);
        if (element.Name != name)
            return ThrowNodeNameMismatch<XmlEndElementNode>(element.Span, name, element.Name);

        return (XmlEndElementNode) Consume();
    }

    public XmlEmptyElementNode ExpectEmptyElement(params string[] names)
    {
        var node = Lookahead;
        if (node is not XmlEmptyElementNode element)
            return ThrowNodeTypeMismatch<XmlEmptyElementNode>(Lookahead, XmlNodeKind.EmptyElement);
        if (!names.Contains(element.Name))
            return ThrowNodeNameMismatch<XmlEmptyElementNode>(element.Span, names, element.Name);

        return (XmlEmptyElementNode) Consume();
    }

    public XmlTextNode ExpectText()
    {
        var node = Lookahead;
        if (node is not XmlTextNode)
            return ThrowNodeTypeMismatch<XmlTextNode>(Lookahead, XmlNodeKind.Text);

        return (XmlTextNode) Consume();
    }

    public XmlNode ExpectElementOrEmptyElement(params string[] names)
    {
        var node = Lookahead;
        return node switch
        {
            XmlElementNode { Name: { } s } element => names.Contains(s)
                ? Consume()
                : ThrowNodeNameMismatch<XmlNode>(element.Span, names, s),
            XmlEmptyElementNode { Name: { } s } emptyElement => names.Contains(s)
                ? Consume()
                : ThrowNodeNameMismatch<XmlNode>(emptyElement.Span, names, s),
            _ => throw new XmlDocParserException(
                $"Expected node type: {XmlNodeKind.Element} or {XmlNodeKind.EmptyElement}, got: {Lookahead?.Kind}",
                Consume().Span)
        };
    }

    public XmlNode Consume()
    {
        if (!IsEof)
            return _nodes[_position++];

        (_, _, int line, int col) = _nodes[^1].Span;
        var span = new TextSpan(line, col + 1, line, col + 1);

        throw new XmlDocParserException("Unexpected end of input", span);
    }

    private void ParseSection(RawXmlDocEntry entry)
    {
        if (Lookahead is not XmlElementNode element)
        {
            ThrowNodeTypeMismatch<object>(Lookahead, XmlNodeKind.Element);
            return;
        }

        // TODO: If we can't find the appropriate section, assume the user wanted to add the content to the summary.
        if (!_sectionParsers.TryGetValue(element.Name, out var sectionParser))
            throw new XmlDocParserException($"Unexpected section: {element.Name}", element.Span);

        sectionParser.ParseSection(this, entry);
    }

    [DoesNotReturn]
    private T ThrowNodeTypeMismatch<T>(XmlNode? node, XmlNodeKind expected)
    {
        (_, _, int line, int col) = _nodes[^1].Span;
        var span = node?.Span ?? new TextSpan(line, col + 1, line, col + 1);
        string got = node?.Kind.ToString() ?? "nothing";

        throw new XmlDocParserException($"Expected node type: {expected}, got: {got}", span);
    }

    [DoesNotReturn]
    private static T ThrowNodeNameMismatch<T>(TextSpan span, string expectedName, string actualName)
    {
        throw new XmlDocParserException($"Expected node with name: '{expectedName}', got '{actualName}'", span);
    }

    [DoesNotReturn]
    private static T ThrowNodeNameMismatch<T>(TextSpan span, string[] expectedNames, string actualName)
    {
        string names = string.Join("' or '", expectedNames);
        throw new XmlDocParserException($"Expected node with name: '{names}', got '{actualName}'", span);
    }
}