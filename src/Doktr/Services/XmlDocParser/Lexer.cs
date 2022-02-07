using System;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Doktr.Services.XmlDocParser;

public abstract record Token;

public record OpenTagToken(string Name, ImmutableDictionary<string, string> Attributes) : Token;

public record CloseTagToken(string Name) : Token;

public record SelfEnclosedTag(string Name, ImmutableDictionary<string, string> Attributes) : Token;

public record TextToken(string Value) : Token;

public class Lexer
{
    private readonly XmlReader _reader;

    public Lexer(string raw)
    {
        _reader = XmlReader.Create(new StringReader(raw), new XmlReaderSettings
        {
            ConformanceLevel = ConformanceLevel.Fragment,
            IgnoreWhitespace = true
        });
    }

    public ImmutableArray<Token> CollectTokens()
    {
        var builder = ImmutableArray.CreateBuilder<Token>();

        while (_reader.Read())
        {
            if (_reader.NodeType == XmlNodeType.XmlDeclaration)
                continue;

            builder.Add(_reader.NodeType switch
            {
                XmlNodeType.Element when !_reader.IsEmptyElement => new OpenTagToken(_reader.Name, CollectAttributes()),
                XmlNodeType.EndElement => new CloseTagToken(_reader.Name),
                XmlNodeType.Element => new SelfEnclosedTag(_reader.Name, CollectAttributes()),
                XmlNodeType.Text => new TextToken(FixWhitespace(_reader.Value)),
                _ => throw new MalformedXmlDocException($"Unexpected xml node type: {_reader.NodeType}")
            });
        }

        return builder.ToImmutable();

        ImmutableDictionary<string, string> CollectAttributes()
        {
            if (_reader.AttributeCount == 0)
                return ImmutableDictionary<string, string>.Empty;
            
            var attributes = ImmutableDictionary.CreateBuilder<string, string>();

            for (int i = 0; i < _reader.AttributeCount; i++)
            {
                _reader.MoveToAttribute(i);
                attributes.Add(_reader.Name, _reader.Value);
            }

            _reader.MoveToElement();
            return attributes.ToImmutable();
        }
    }

    private string FixWhitespace(string value)
    {
        string[] lines = value.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return string.Join(' ', lines);
    }
}