using System;
using System.Collections.Immutable;
using System.IO;
using System.Xml;
using Doktr.Models.Segments;
using Doktr.Services.DocumentationParsing;

namespace Doktr.Services
{
    public class XmlParserService : IXmlParserService
    {
        private static readonly XmlReaderSettings XmlSettings = new()
        {
            ConformanceLevel = ConformanceLevel.Fragment,
            IgnoreComments = true,
            IgnoreWhitespace = true,
            IgnoreProcessingInstructions = true
        };

        private readonly XmlReader _reader;
        private readonly IDocumentationSegmentParserProvider _parserProvider;
        private readonly ImmutableDictionary<string, XmlDocEntry>.Builder _documentation;

        public XmlParserService(string raw, IDocumentationSegmentParserProvider parserProvider)
        {
            _reader = XmlReader.Create(new StringReader(raw), XmlSettings);
            _parserProvider = parserProvider;
            _documentation = ImmutableDictionary.CreateBuilder<string, XmlDocEntry>();
        }

        public ImmutableDictionary<string, XmlDocEntry> ParseXmlFile()
        {
            while (_reader.Read())
            {
                if (_reader.NodeType != XmlNodeType.Element || _reader.Name != "member")
                    continue;

                string docId = _reader["name"] ?? throw new Exception("Malformed XMLDOC");
                string raw = _reader.ReadInnerXml();
                _documentation[docId] = ParseDocumentation(raw);
            }

            return _documentation.ToImmutable();
        }

        private XmlDocEntry ParseDocumentation(string raw)
        {
            using var reader = XmlReader.Create(new StringReader(raw), XmlSettings);
            var summary = ImmutableArray.CreateBuilder<IDocumentationSegment>();
            var parameters = ImmutableDictionary.CreateBuilder<string, IDocumentationSegment>();
            var typeParameters = ImmutableDictionary.CreateBuilder<string, IDocumentationSegment>();
            var exceptions = ImmutableDictionary.CreateBuilder<string, IDocumentationSegment>();
            var returns = ImmutableArray.CreateBuilder<IDocumentationSegment>();
            var examples = ImmutableArray.CreateBuilder<IDocumentationSegment>();
            var remarks = ImmutableArray.CreateBuilder<IDocumentationSegment>();
            var seealso = ImmutableArray.CreateBuilder<string>();

            return new XmlDocEntry(summary.ToImmutable(),
                parameters.ToImmutable(),
                typeParameters.ToImmutable(),
                exceptions.ToImmutable(),
                returns.ToImmutable(),
                examples.ToImmutable(),
                remarks.ToImmutable(),
                seealso.ToImmutable());
        }
    }
}