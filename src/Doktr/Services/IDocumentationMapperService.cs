using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Dependencies;

namespace Doktr.Services;

public interface IDocumentationMapperService
{
    ImmutableDictionary<IFullNameProvider, XmlDocEntry> MapDocumentation(
        DependencyGraph dependencyGraph,
        ImmutableDictionary<string, XmlDocEntry> documentation);
}