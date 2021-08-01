using System.Collections.Immutable;

namespace Doktr.Services
{
    public interface IXmlParserService
    {
        ImmutableDictionary<string, XmlDocEntry> ParseXmlFile();
    }
}