using System.Collections.Generic;
using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Serilog;

namespace Doktr.Services;

public class InheritDocResolutionService : IInheritDocResolutionService
{
    private readonly ILogger _logger;
    private readonly HashSet<XmlDocEntry> _visited;

    public InheritDocResolutionService(ILogger logger)
    {
        _logger = logger;
        _visited = new();
    }

    public ImmutableDictionary<IFullNameProvider, XmlDocEntry> ResolveInheritance(
        DependencyGraph dependencyGraph,
        ImmutableDictionary<string, XmlDocEntry> raw,
        ImmutableDictionary<IFullNameProvider, XmlDocEntry> mapped)
    {
        var builder = ImmutableDictionary.CreateBuilder<IFullNameProvider, XmlDocEntry>();

        foreach (var (member, entry) in mapped)
        {
            _visited.Clear();
            var node = dependencyGraph.Mapping[member];
            if (dependencyGraph.Roots.Contains(node) || entry.InheritFrom is null)
            {
                builder.Add(member, entry);
                continue;
            }

            var resolved = entry;
            bool good = true;
            while (resolved.InheritFrom is not null && good)
            {
                resolved = resolved.InheritFrom == ""
                    ? Implicit(member, entry, dependencyGraph, mapped, ref good)
                    : Explicit(member, entry, raw, ref good);
            }

            if (resolved.InheritFrom is not null)
                _logger.Warning("Couldn't resolve inheritdoc of {Member} fully", member);

            builder.Add(member, resolved);
        }

        return builder.ToImmutable();
    }

    private XmlDocEntry Implicit(
        IFullNameProvider member,
        XmlDocEntry entry,
        DependencyGraph dependencyGraph,
        ImmutableDictionary<IFullNameProvider, XmlDocEntry> mapped,
        ref bool good)
    {
        var agenda = new Queue<DependencyNode>();
        agenda.Enqueue(dependencyGraph.Mapping[member]);
        string? from = entry.InheritFrom;
        while (from == "" && agenda.TryDequeue(out var current))
        {
            foreach (var dep in current.Dependencies)
                agenda.Enqueue(dep);

            if (!mapped.TryGetValue(current.MetadataMember, out var parent) || member == current.MetadataMember)
                continue;

            if (!_visited.Add(parent))
            {
                _logger.Error("Cyclic dependency detected while trying to resolve inheritdoc of {Member}", member);
                good = false;
                return entry;
            }

            entry = CombineEntries(entry, parent);
            from = entry.InheritFrom;
            if (from is null)
                return entry;
        }

        good = false;
        return entry;
    }

    private XmlDocEntry Explicit(
        IFullNameProvider member,
        XmlDocEntry entry,
        ImmutableDictionary<string, XmlDocEntry> raw,
        ref bool good)
    {
        string? from = entry.InheritFrom;
        while (from is not null or "")
        {
            if (!raw.TryGetValue(from, out var parent))
            {
                _logger.Warning("Couldn't find documentation with id '{DocId}' to explicitly inherit from in {Member}",
                    from, member);
                continue;
            }

            if (!_visited.Add(parent))
            {
                good = false;
                _logger.Error("Cyclic dependency detected while trying to resolve inheritdoc of {Member}", member);
                return entry;
            }

            entry = CombineEntries(entry, parent);
            from = entry.InheritFrom;
            if (from is null)
                return entry;
        }

        good = false;
        return entry;
    }

    private XmlDocEntry CombineEntries(XmlDocEntry current, XmlDocEntry parent)
    {
        return new XmlDocEntry(
            parent.InheritFrom,
            current.Summary.IsEmpty ? parent.Summary : current.Summary,
            Combine(current.Parameters, parent.Parameters),
            Combine(current.TypeParameters, parent.TypeParameters),
            Combine(current.Exceptions, parent.Exceptions),
            current.Returns.IsEmpty ? parent.Returns : current.Returns,
            current.Examples.IsEmpty ? parent.Examples : current.Examples,
            current.Remarks.IsEmpty ? parent.Remarks : current.Remarks,
            current.Seealso.IsEmpty ? parent.Seealso : current.Seealso.AddRange(parent.Seealso)
        );

        static ImmutableDictionary<string, T> Combine<T>(
            ImmutableDictionary<string, T> current,
            ImmutableDictionary<string, T> parent)
        {
            if (current.IsEmpty)
                return parent;

            var builder = ImmutableDictionary.CreateBuilder<string, T>();

            builder.AddRange(parent);
            foreach (var (key, value) in current)
                builder[key] = value;

            return builder.ToImmutable();
        }
    }
}