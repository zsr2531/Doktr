using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public class BoldSegmentParser : IDocumentationSegmentParser
    {
        private readonly IDocumentationSegmentParserProvider _parserProvider;

        public BoldSegmentParser(IDocumentationSegmentParserProvider parserProvider)
        {
            _parserProvider = parserProvider;
        }

        public IEnumerable<string> GetTagNames() => new[]
        {
            "b", "strong", "em"
        };

        public IDocumentationSegment Parse(XmlReader reader)
        {
            reader.Skip(); // Skip the starting element.
            var builder = ImmutableArray.CreateBuilder<IDocumentationSegment>();
            
            while (reader.NodeType != XmlNodeType.EndElement)
                builder.Add(_parserProvider.ParseNextSegment(reader));

            reader.Skip(); // Skip the ending element.
            return new BoldDocumentationSegment(builder.ToImmutable());
        }
    }
}