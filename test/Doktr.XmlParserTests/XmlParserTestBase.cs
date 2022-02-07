using System.Collections.Immutable;
using System.Linq;
using Doktr.Services;
using Doktr.Services.XmlDocParser;

namespace Doktr.XmlParserTests;

public abstract class XmlParserTestBase
{
    private const string Template = @"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Bali</name>
    </assembly>
    <members>
        <member name=""T:Test"">
            {0}
        </member>
    </members>
</doc>
";
    
    public ImmutableDictionary<string, XmlDocEntry> ParseXml(string xml, out ImmutableArray<string> diagnostics)
    {
        var lexer = new Lexer(string.Format(Template, string.Join("\n            ", xml.Split("\n"))));
        var tokens = lexer.CollectTokens();
        var parser = new Parser(tokens);

        return parser.Parse(out diagnostics);
    }

    public XmlDocEntry ParseSingleXml(string xml, out ImmutableArray<string> diagnostics)
    {
        var results = ParseXml(xml, out diagnostics);
        var entry = results.Values.First();

        return entry;
    }
}