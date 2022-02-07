using System.Collections.Immutable;
using Doktr.Services.XmlDocParser;
using Serilog;

namespace Doktr.Services;

public class XmlParserService : IXmlParserService
{
    private readonly ILogger _logger;

    public XmlParserService(ILogger logger)
    {
        _logger = logger;
    }

    public ImmutableDictionary<string, XmlDocEntry> ParseXmlFile(string raw)
    {
        var tokens = new Lexer(raw).CollectTokens();
        var parsed = new Parser(tokens).Parse(out var diagnostics);
        foreach (string diag in diagnostics)
            _logger.Error("Error while parsing xmldoc: '{}'", diag);

        return parsed;
    }
}