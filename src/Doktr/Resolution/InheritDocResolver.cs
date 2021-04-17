using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Analysis;
using Doktr.Xml;

namespace Doktr.Resolution
{
    public class InheritDocResolver
    {
        private readonly IDictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>> _docs;
        private readonly DependencyGraph _dependencyGraph;
        private readonly IReadOnlyDictionary<string, ImmutableArray<IXmlDocSegment>> _raw;

        public InheritDocResolver(
            IDictionary<IFullNameProvider, ImmutableArray<IXmlDocSegment>> docs,
            DependencyGraph dependencyGraph,
            IReadOnlyDictionary<string, ImmutableArray<IXmlDocSegment>> raw)
        {
            _docs = docs;
            _dependencyGraph = dependencyGraph;
            _raw = raw;
        }

        public void ResolveInheritDoc()
        {
            foreach (var (member, _) in _docs)
            {
                if (!InheritsDoc(member))
                    continue;
                
                var inherit = (InheritDocXmlDocSegment) _docs[member][0];
                if (inherit.From is not null)
                {
                    if (_raw.TryGetValue(inherit.From, out var resolved))
                    {
                        _docs[member] = resolved;
                        continue;
                    }
                    
                    Logger.Warning($"Couldn't resolve explicit inheritdoc of member {member} ('{inherit.From}')");
                }

                var inherited = TryInheritDoc(member);
                if (inherited is null)
                {
                    Logger.Warning($"Couldn't resolve inheritdoc of member {member}");
                    continue;
                }

                _docs[member] = inherited.Value;
            }
        }

        private ImmutableArray<IXmlDocSegment>? TryInheritDoc(IFullNameProvider member)
        {
            var agenda = new Queue<IFullNameProvider>();
            foreach (var dependency in _dependencyGraph.Nodes[member].Dependencies)
                agenda.Enqueue(dependency.MetadataMember);

            while (agenda.Count > 0)
            {
                EnsureNoDuplicates(member, agenda);
                
                var candidate = agenda.Dequeue();
                if (!InheritsDoc(candidate) && _docs.TryGetValue(candidate, out var resolved))
                    return resolved;
                
                foreach (var dependency in _dependencyGraph.Nodes[candidate].Dependencies)
                    agenda.Enqueue(dependency.MetadataMember);
            }

            return null;
        }

        private bool InheritsDoc(IFullNameProvider member)
        {
            if (!_docs.TryGetValue(member, out var docs))
                return false;
            if (docs.Length > 1)
                return false;

            var segment = docs[0];
            return segment is InheritDocXmlDocSegment;
        }

        private void EnsureNoDuplicates(IFullNameProvider root, Queue<IFullNameProvider> agenda)
        {
            var set = new HashSet<IFullNameProvider>();
            foreach (var item in agenda)
            {
                if (!set.Add(item))
                    throw new Exception($"Cyclic inheritdoc dependency encountered while processing {root}");
            }
        }
    }
}