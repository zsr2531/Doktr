namespace Doktr.Services
{
    public interface IXmlParserServiceFactory
    {
        IXmlParserService CreateXmlParserService(string path);
    }
}