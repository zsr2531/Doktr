using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Xml;

namespace Doktr.Xml
{
    public class DocumentationReader
    {
        private readonly Dictionary<string, ImmutableArray<IXmlDocSegment>> _loaded;

        public DocumentationReader(string path)
        {
            _loaded = Read(path);
        }

        public IReadOnlyDictionary<string, ImmutableArray<IXmlDocSegment>> Loaded => _loaded;

        private static Dictionary<string, ImmutableArray<IXmlDocSegment>> Read(string path)
        {
            var dictionary = new Dictionary<string, ImmutableArray<IXmlDocSegment>>();
            using var stream = File.OpenRead(path);
            using var reader = XmlReader.Create(stream);
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element || reader.Name != "member")
                    continue;

                string raw = reader["name"];
                var segmentator = new XmlDocSegmentator(reader.ReadInnerXml());
                dictionary[raw ?? throw new InvalidOperationException("Malformed XMLDOC")] = segmentator.BuildSegments();
            }

            return dictionary;
        }
    }
}