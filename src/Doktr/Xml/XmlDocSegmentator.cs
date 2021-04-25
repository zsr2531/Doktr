using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Xml;

namespace Doktr.Xml
{
    public class XmlDocSegmentator : IDisposable
    {
        private readonly XmlReader _reader;

        private static readonly XmlReaderSettings Settings = new()
        {
            ConformanceLevel = ConformanceLevel.Fragment
        };

        public XmlDocSegmentator(string xml)
        {
            _reader = XmlReader.Create(new StringReader(xml), Settings);
        }

        public ImmutableArray<IXmlDocSegment> BuildSegments()
        {
            var builder = ImmutableArray.CreateBuilder<IXmlDocSegment>();

            while (_reader.Read())
            {
                if (_reader.NodeType == XmlNodeType.Whitespace)
                    continue;
                
                builder.Add(_reader.NodeType switch
                {
                    XmlNodeType.Element => ProcessElement(),
                    _ => throw new ArgumentOutOfRangeException()
                });
            }

            return builder.ToImmutable();
        }

        private IXmlDocSegment ProcessElement()
        {
            string type = _reader.Name;
            string name = _reader["name"];
            string cref = _reader["cref"];
            string typeType = _reader["type"];
            if (_reader.IsEmptyElement)
            {
                return type switch
                {
                    "inheritdoc" => new InheritDocXmlDocSegment(_reader["cref"]),
                    "see" => new SeeXmlDocSegment(_reader["cref"]),
                    "seealso" => new SeeXmlDocSegment(_reader["cref"]),
                    "paramref" => new ParamrefXmlDocSegment(_reader["name"]),
                    "typeparamref" => new TypeParamrefXmlDocSegment(_reader["name"]),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            var content = ImmutableArray.CreateBuilder<IXmlDocSegment>();

            while (_reader.Read())
            {
                if (_reader.NodeType == XmlNodeType.Whitespace)
                    continue;
                
                if (_reader.NodeType == XmlNodeType.Text)
                    content.Add(new RawXmlDocSegment(FixWhitespace(_reader.Value)));
                else if (_reader.NodeType == XmlNodeType.Element)
                    content.Add(ProcessElement());
                else if (_reader.NodeType == XmlNodeType.EndElement)
                    break;
            }

            return type switch
            {
                "summary" => new SummaryXmlDocSegment(content.ToImmutable()),
                "param" => new ParamXmlDocSegment(name, content.ToImmutable()),
                "typeparam" => new TypeParamXmlDocSegment(name, content.ToImmutable()),
                "returns" => new ReturnsXmlDocSegment(content.ToImmutable()),
                "remarks" => new RemarksXmlDocSegment(content.ToImmutable()),
                "exception" => new ExceptionXmlDocSegment(cref, content.ToImmutable()),
                "list" => new ListXmlDocSegment(typeType, content.ToImmutable()),
                "item" => new ItemXmlDocSegment(content.ToImmutable()),
                "description" => new DescriptionXmlDocSegment(content.ToImmutable()),
                "para" => new ParaXmlDocSegment(content.ToImmutable()),
                "c" => new MonospaceXmlDocSegment(content.ToImmutable()),
                "strong" => new StrongXmlDocSegment(content.ToImmutable()),
                "i" => null, // TODO
                "b" => null, // TODO
                "value" => null, // TODO
                "term" => null, // TODO
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        private static string FixWhitespace(string value)
        {
            var sb = new StringBuilder();

            int i = 0;
            while (i < value.Length)
            {
                if (!char.IsWhiteSpace(value[i]) || i + 1 >= value.Length || !char.IsWhiteSpace(value[i + 1]))
                    sb.Append(value[i]);

                i++;
            }

            return sb.ToString();
        }
    }
}