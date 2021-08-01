using System.Collections.Generic;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public class CodeSegmentParser : IDocumentationSegmentParser
    {
        public IEnumerable<string> GetTagNames() => new[]
        {
            "code"
        };

        public IDocumentationSegment Parse(XmlReader reader)
        {
            reader.Skip(); // Skip the starting element.
            string content = reader.ReadInnerXml();
            reader.Skip(); // Skip the ending element.
            
            return new CodeBlockDocumentationSegment(content);
        }
    }
}