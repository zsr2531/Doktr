using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Doktr.Models;

namespace Doktr.Services;

public interface ISemanticDocumentationValidator
{
    ImmutableArray<ImmutableArray<TypeDocumentation>> ValidateDocumentation(
        DependencyGraph dependencyGraph,
        ImmutableDictionary<IFullNameProvider, XmlDocEntry> documentation);
}