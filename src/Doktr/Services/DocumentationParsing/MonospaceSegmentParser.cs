using System.Collections.Generic;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public class MonospaceSegmentParser : IDocumentationSegmentParser
    {
        public IEnumerable<string> GetTagNames() => new[]
        {
            "c"
        };

        public IDocumentationSegment Parse(XmlReader reader)
        {
            reader.Skip(); // Skip the starting element.
            string content = reader.ReadInnerXml();
            reader.Skip(); // Skip the ending element.

            return new MonospaceDocumentationSegment(content);
        }
    }
}