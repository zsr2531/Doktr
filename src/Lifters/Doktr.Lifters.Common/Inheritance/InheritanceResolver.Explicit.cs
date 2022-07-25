using Doktr.Xml.XmlDoc;

namespace Doktr.Lifters.Common.Inheritance;

public partial class InheritanceResolver<T>
{
    private RawXmlDocEntry ResolveExplicitly(RawXmlDocEntry entry)
    {
        var docId = entry.DocId;
        var from = entry.InheritsDocumentationExplicitlyFrom!.Value;
        if (TryGetEntry(from, out var found))
        {
            _logger.Verbose("Successfully resolved explicit inheritdoc in {Member}: {Target}", docId, from);
            var resolved = ResolveInheritance(found);
            return resolved;
        }

        _logger.Warning("Failed to resolve explicit inheritdoc in {Member}: {Target}", docId, from);
        entry.InheritsDocumentationExplicitlyFrom = null;
        return entry;
    }
}