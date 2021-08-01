using System.Collections.Generic;
using System.Xml;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public interface IDocumentationSegmentParser
    {
        IEnumerable<string> GetTagNames();
        
        IDocumentationSegment Parse(XmlReader reader);
    }
}