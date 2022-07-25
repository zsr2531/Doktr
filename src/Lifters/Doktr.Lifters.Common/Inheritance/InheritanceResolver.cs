using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Doktr.Core.Models;
using Doktr.Lifters.Common.DependencyGraph;
using Doktr.Xml.XmlDoc;
using Doktr.Xml.XmlDoc.Collections;
using Serilog;

namespace Doktr.Lifters.Common.Inheritance;

public partial class InheritanceResolver<T> : IInheritanceResolver<T>
    where T : notnull
{
    private readonly ILogger _logger;
    private readonly DependencyGraph<T> _depGraph;
    private readonly RawXmlDocEntryMap _docs;
    private readonly RawXmlDocEntryMap _cache = new();
    private readonly Queue<CodeReference> _agenda = new();

    public InheritanceResolver(ILogger logger, DependencyGraph<T> depGraph, RawXmlDocEntryMap docs)
    {
        _logger = logger;
        _depGraph = depGraph;
        _docs = docs;
    }

    public RawXmlDocEntry ResolveInheritance(RawXmlDocEntry entry)
    {
        if (!entry.InheritsDocumentation)
            return entry;

        _agenda.Enqueue(entry.DocId);
        AssertNoCycles();

        var resolved = entry.InheritsDocumentationImplicitly
            ? ResolveImplicitly(entry)
            : ResolveExplicitly(entry);

        var fromAgenda = _agenda.Dequeue();
        Debug.Assert(entry.DocId == fromAgenda);

        SetEntry(entry.DocId, resolved);
        return resolved;
    }

    private bool TryGetEntry(CodeReference codeRef, [NotNullWhen(true)] out RawXmlDocEntry? entry)
    {
        if (_cache.TryGetValue(codeRef, out entry))
            return true;
        if (_docs.TryGetValue(codeRef, out entry))
            return true;

        entry = null;
        return false;
    }

    private void SetEntry(CodeReference codeRef, RawXmlDocEntry entry) => _cache[codeRef] = entry;

    private void AssertNoCycles()
    {
        if (_agenda.Count == _agenda.Distinct().Count())
            return;

        var codeRefs = _agenda.Reverse().ToArray();
        throw new InheritanceResolutionException("Encountered a cycle during inheritance resolution", codeRefs);
    }
}