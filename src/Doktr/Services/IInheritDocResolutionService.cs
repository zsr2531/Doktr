using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Dependencies;

namespace Doktr.Services;

public interface IInheritDocResolutionService
{
    ImmutableDictionary<IFullNameProvider, XmlDocEntry> ResolveInheritance(
        DependencyGraph dependencyGraph,
        ImmutableDictionary<string, XmlDocEntry> raw,
        ImmutableDictionary<IFullNameProvider, XmlDocEntry> mapped);
}