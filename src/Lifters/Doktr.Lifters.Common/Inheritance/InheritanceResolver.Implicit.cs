using Doktr.Xml.XmlDoc;

namespace Doktr.Lifters.Common.Inheritance;

public partial class InheritanceResolver<T>
{
    private RawXmlDocEntry ResolveImplicitly(RawXmlDocEntry entry)
    {
        var docId = entry.DocId;

        return entry;
    }
}