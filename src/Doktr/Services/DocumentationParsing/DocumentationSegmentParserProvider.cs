using System.Collections.Immutable;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public class DocumentationSegmentParserProvider : IDocumentationSegmentParserProvider
    {
        public DocumentationSegmentParserProvider()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, IDocumentationSegmentParser>();
            
            AddToBuilder(new ParagraphSegmentParser(this));
            AddToBuilder(new ItalicSegmentParser(this));
            AddToBuilder(new BoldSegmentParser(this));
            AddToBuilder(new CodeSegmentParser());
            AddToBuilder(new MonospaceSegmentParser());

            Parsers = builder.ToImmutable();

            void AddToBuilder(IDocumentationSegmentParser parser)
            {
                foreach (string tagName in parser.GetTagNames())
                    builder.Add(tagName, parser);
            }
        }
        
        public ImmutableDictionary<string, IDocumentationSegmentParser> Parsers
        {
            get;
        }

        public IDocumentationSegment ParseNextSegment(XmlReader reader)
        {
            reader.Read();
            return reader.NodeType == XmlNodeType.Text
                ? new TextDocumentationSegment(reader.Value)
                : Parsers[reader.Name].Parse(reader);
        }
    }
}