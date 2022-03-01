using System;
using System.Collections.Immutable;
using System.Linq;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Services.XmlDocParser;

public class Parser
{
    private readonly ImmutableArray<Token> _tokens;
    private readonly ImmutableArray<string>.Builder _diagnostics;
    private int _position;

    public Parser(ImmutableArray<Token> tokens)
    {
        _tokens = tokens;
        _diagnostics = ImmutableArray.CreateBuilder<string>();
    }

    private Token? Lookahead => _position >= _tokens.Length
        ? null
        : _tokens[_position];

    public ImmutableDictionary<string, XmlDocEntry> Parse(out ImmutableArray<string> diagnostics)
    {
        var builder = ImmutableDictionary.CreateBuilder<string, XmlDocEntry>();

        MatchOpeningTag("doc");
        MatchOpeningTag("assembly");
        MatchOpeningTag("name");
        MatchText();
        MatchClosingTag("name");
        MatchClosingTag("assembly");
        MatchOpeningTag("members");

        while (Lookahead is OpenTagToken && MatchOpeningTag("member").Attributes.TryGetValue("name", out string? id))
        {
            var entry = ParseEntry();
            builder.Add(id, entry);
            MatchClosingTag("member");
        }

        MatchClosingTag("members");
        MatchClosingTag("doc");

        diagnostics = _diagnostics.ToImmutable();
        return builder.ToImmutable();
    }

    private XmlDocEntry ParseEntry()
    {
        string? inheritFrom = null;
        var summary = ImmutableArray.CreateBuilder<IDocumentationSegment>();
        var parameters = ImmutableDictionary.CreateBuilder<string, ImmutableArray<IDocumentationSegment>>();
        var typeParameters = ImmutableDictionary.CreateBuilder<string, ImmutableArray<IDocumentationSegment>>();
        var exceptions = ImmutableDictionary.CreateBuilder<string, ImmutableArray<IDocumentationSegment>>();
        var returns = ImmutableArray.CreateBuilder<IDocumentationSegment>();
        var examples = ImmutableArray.CreateBuilder<IDocumentationSegment>();
        var remarks = ImmutableArray.CreateBuilder<IDocumentationSegment>();
        var seealso = ImmutableArray.CreateBuilder<string>();

        while (Lookahead is not CloseTagToken)
        {
            var current = Consume();
            if (current is TextToken text)
            {
                summary.Add(new TextDocumentationSegment(text.Value));
                continue;
            }

            if (current is SelfEnclosedTag { Name: "seealso" } self)
            {
                seealso.Add(self.Attributes["cref"]);
                continue;
            }

            if (current is SelfEnclosedTag { Name: "inheritdoc" } inherit)
            {
                inheritFrom = inherit.Attributes.TryGetValue("cref", out string? cref)
                    ? cref
                    : "";
                continue;
            }

            if (current is not OpenTagToken open)
            {
                _diagnostics.Add($"Unexpected xmldoc tag: '{current}'");
                continue;
            }

            switch (open.Name)
            {
                case "summary":
                    summary.AddRange(ParseBody());
                    break;

                case "param":
                    string paramName = open.Attributes["name"];
                    parameters.Add(paramName, ParseBody());
                    break;

                case "typeparam":
                    string typeParamName = open.Attributes["name"];
                    typeParameters.Add(typeParamName, ParseBody());
                    break;

                case "exception":
                    string ex = open.Attributes["cref"];
                    exceptions.Add(ex, ParseBody());
                    break;

                case "returns":
                    returns.AddRange(ParseBody());
                    break;

                case "example":
                    examples.AddRange(ParseBody());
                    break;

                case "remarks":
                    remarks.AddRange(ParseBody());
                    break;

                // case "value": break; // TODO
                default:
                    _diagnostics.Add($"Unknown xmldoc tag: '{open.Name}'");
                    continue;
            }

            MatchClosingTag(open.Name);
        }

        return new XmlDocEntry(
            inheritFrom,
            summary.ToImmutable(),
            parameters.ToImmutable(),
            typeParameters.ToImmutable(),
            exceptions.ToImmutable(),
            returns.ToImmutable(),
            examples.ToImmutable(),
            remarks.ToImmutable(),
            seealso.ToImmutable()
        );
    }

    private ImmutableArray<IDocumentationSegment> ParseBody()
    {
        var builder = ImmutableArray.CreateBuilder<IDocumentationSegment>();

        while (Lookahead is not CloseTagToken)
        {
            var current = Consume();
            if (current is TextToken text)
            {
                builder.Add(new TextDocumentationSegment(text.Value));
                continue;
            }

            if (current is SelfEnclosedTag { Name: "see" } see)
            {
                if (see.Attributes.TryGetValue("cref", out string? cref))
                    builder.Add(new ReferenceDocumentationSegment(new RawReference(cref, cref)));
                else if (see.Attributes.TryGetValue("href", out string? url))
                    builder.Add(new LinkDocumentationSegment(url));
                else if (see.Attributes.TryGetValue("langword", out string? langword))
                    builder.Add(new MonospaceDocumentationSegment(langword));
                else
                    _diagnostics.Add($"Unknown attribute on see tag: '{see.Attributes}'");

                continue;
            }

            if (current is SelfEnclosedTag { Name: "paramref" } paramref)
            {
                builder.Add(new ItalicDocumentationSegment(
                    ImmutableArray.Create<IDocumentationSegment>(
                        new TextDocumentationSegment(paramref.Attributes["name"]))));
                continue;
            }

            if (current is SelfEnclosedTag { Name: "typeparamref" } typeparamref)
            {
                builder.Add(new ItalicDocumentationSegment(
                    ImmutableArray.Create<IDocumentationSegment>(
                        new TextDocumentationSegment(typeparamref.Attributes["name"]))));
                continue;
            }

            var open = (OpenTagToken) current;
            switch (open.Name)
            {
                case "p" or "para":
                    builder.Add(new ParagraphDocumentationSegment(ParseBody()));
                    break;

                case "b" or "strong":
                    builder.Add(new BoldDocumentationSegment(ParseBody()));
                    break;

                case "i" or "italic":
                    builder.Add(new ItalicDocumentationSegment(ParseBody()));
                    break;

                case "c":
                    builder.Add(new MonospaceDocumentationSegment(string.Join(" ",
                        ParseBody().OfType<TextDocumentationSegment>().Select(s => s.Content))));
                    break;

                case "code":
                    builder.Add(new CodeBlockDocumentationSegment(string.Join(" ",
                        ParseBody().OfType<TextDocumentationSegment>().Select(s => s.Content))));
                    break;

                case "list":
                    string type = open.Attributes["type"];
                    builder.Add(type switch
                    {
                        "bullet" => ParseList(ListType.Bullet),
                        "numbered" => ParseList(ListType.Numbered),
                        "table" => ParseTable(),
                        _ => throw new MalformedXmlDocException($"Unknown list type {type}")
                    });
                    break;
            }

            MatchClosingTag(open.Name);
        }

        return builder.ToImmutable();
    }

    private ListDocumentationSegment ParseList(ListType type)
    {
        var items = ImmutableArray.CreateBuilder<ImmutableArray<IDocumentationSegment>>();

        while (Lookahead is OpenTagToken { Name: "item" })
        {
            Consume();
            items.Add(ParseBody());
            MatchClosingTag("item");
        }

        return new ListDocumentationSegment(type, items.ToImmutable());
    }

    private TableDocumentationSegment ParseTable()
    {
        var header = ImmutableArray.CreateBuilder<ImmutableArray<IDocumentationSegment>>();
        MatchOpeningTag("listheader");
        while (Lookahead is OpenTagToken { Name: "term" })
        {
            Consume();
            header.Add(ParseBody());
            MatchClosingTag("term");
        }

        MatchClosingTag("listheader");

        var rows = ImmutableArray.CreateBuilder<ImmutableArray<ImmutableArray<IDocumentationSegment>>>();
        while (Lookahead is OpenTagToken { Name: "item" })
        {
            Consume();
            var row = ImmutableArray.CreateBuilder<ImmutableArray<IDocumentationSegment>>();

            while (Lookahead is OpenTagToken { Name: "term" })
            {
                Consume();
                row.Add(ParseBody());
                MatchClosingTag("term");
            }

            rows.Add(row.ToImmutable());
            MatchClosingTag("item");
        }

        return new TableDocumentationSegment(header.ToImmutable(), rows.ToImmutable());
    }

    private TextToken MatchText()
    {
        if (Lookahead is TextToken)
            return (TextToken) Consume();

        _diagnostics.Add($"Expected text, instead got: {(Lookahead is null ? "nothing" : Lookahead)}");
        return new TextToken("<missing text>");
    }

    private OpenTagToken MatchOpeningTag(string name)
    {
        if (Lookahead is OpenTagToken open && open.Name == name)
            return (OpenTagToken) Consume();

        _diagnostics.Add(
            $"Expected opening tag with name '{name}', instead got: {(Lookahead is null ? "nothing" : Lookahead)}");
        return new OpenTagToken(name, ImmutableDictionary<string, string>.Empty);
    }

    private CloseTagToken MatchClosingTag(string name)
    {
        if (Lookahead is CloseTagToken close && close.Name == name)
            return (CloseTagToken) Consume();

        _diagnostics.Add(
            $"Expected closing tag with name '{name}', instead got: {(Lookahead is null ? "nothing" : Lookahead)}");
        return new CloseTagToken(name);
    }

    private SelfEnclosedTag MatchSelfEnclosingTag(string name)
    {
        if (Lookahead is SelfEnclosedTag self && self.Name == name)
            return (SelfEnclosedTag) Consume();

        _diagnostics.Add(
            $"Expected self enclosing tag with name '{name}', instead got: {(Lookahead is null ? "nothing" : Lookahead)}");
        return new SelfEnclosedTag(name, ImmutableDictionary<string, string>.Empty);
    }

    private Token Consume()
    {
        if (_position >= _tokens.Length)
            throw new InvalidOperationException();

        return _tokens[_position++];
    }
}