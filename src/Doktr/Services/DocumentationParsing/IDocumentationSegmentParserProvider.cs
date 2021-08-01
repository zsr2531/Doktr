using System.Collections.Immutable;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public interface IDocumentationSegmentParserProvider
    {
        ImmutableDictionary<string, IDocumentationSegmentParser> Parsers
        {
            get;
        }

        IDocumentationSegment ParseNextSegment(XmlReader reader);
    }
}