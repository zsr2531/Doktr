using System.IO;
using Doktr.Services.DocumentationParsing;

namespace Doktr.Services
{
    public class XmlParserServiceFactory : IXmlParserServiceFactory
    {
        private readonly IDocumentationSegmentParserProvider _parserProvider;

        public XmlParserServiceFactory(IDocumentationSegmentParserProvider parserProvider)
        {
            _parserProvider = parserProvider;
        }

        public IXmlParserService CreateXmlParserService(string path)
        {
            string contents = File.ReadAllText(path);
            return new XmlParserService(contents, _parserProvider);
        }
    }
}