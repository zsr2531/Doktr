using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public class ItalicSegmentParser : IDocumentationSegmentParser
    {
        private readonly IDocumentationSegmentParserProvider _parserProvider;

        public ItalicSegmentParser(IDocumentationSegmentParserProvider parserProvider)
        {
            _parserProvider = parserProvider;
        }

        public IEnumerable<string> GetTagNames() => new[]
        {
            "i"
        };

        public IDocumentationSegment Parse(XmlReader reader)
        {
            reader.Skip(); // Skip the starting element.
            var builder = ImmutableArray.CreateBuilder<IDocumentationSegment>();
            
            while (reader.NodeType != XmlNodeType.EndElement)
                builder.Add(_parserProvider.ParseNextSegment(reader));

            reader.Skip(); // Skip the ending element.
            return new ItalicDocumentationSegment(builder.ToImmutable());
        }
    }
}