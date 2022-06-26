using Doktr.Core.Models;

namespace Doktr.Xml.XmlDoc.Collections;

public class RawXmlDocEntryMap : Dictionary<CodeReference, RawXmlDocEntry>
{
    public void MergeWith(RawXmlDocEntryMap other)
    {
        foreach (var entry in other)
            Add(entry.Key, entry.Value);
    }
}