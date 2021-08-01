using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public class ParagraphSegmentParser : IDocumentationSegmentParser
    {
        private readonly IDocumentationSegmentParserProvider _parserProvider;

        public ParagraphSegmentParser(IDocumentationSegmentParserProvider parserProvider)
        {
            _parserProvider = parserProvider;
        }

        public IEnumerable<string> GetTagNames() => new[]
        {
            "p", "para"
        };

        public IDocumentationSegment Parse(XmlReader reader)
        {
            reader.Skip(); // Skip the starting element.
            var builder = ImmutableArray.CreateBuilder<IDocumentationSegment>();
            
            while (reader.NodeType != XmlNodeType.EndElement)
                builder.Add(_parserProvider.ParseNextSegment(reader));

            reader.Skip(); // Skip the ending element.
            return new ParagraphDocumentationSegment(builder.ToImmutable());
        }
    }
}