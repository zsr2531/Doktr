using Doktr.Xml.XmlDoc;

namespace Doktr.Lifters.Common.Inheritance;

public interface IInheritanceResolver<T>
    where T : notnull
{
    RawXmlDocEntry ResolveInheritance(RawXmlDocEntry entry);
}