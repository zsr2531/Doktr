using System;
using System.Collections.Generic;
using System.Xml;
using Doktr.Models.References;
using Doktr.Models.Segments;

namespace Doktr.Services.DocumentationParsing
{
    public class ReferenceSegmentParser : IDocumentationSegmentParser
    {
        public IEnumerable<string> GetTagNames() => new[]
        {
            "see"
        };

        public IDocumentationSegment Parse(XmlReader reader)
        {
            if (reader["cref"] is { } cref)
                return new ReferenceDocumentationSegment(new RawReference(cref, cref));
            if (reader["langword"] is { } langword)
                return new MonospaceDocumentationSegment(langword);

            throw new NotSupportedException("'see' has no cref or langword.");
        }
    }
}