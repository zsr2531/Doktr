namespace Doktr.Lifters.Common.XmlDoc;

public interface IXmlDocParserFactory
{
    XmlDocParser CreateParser(TextReader reader);
}