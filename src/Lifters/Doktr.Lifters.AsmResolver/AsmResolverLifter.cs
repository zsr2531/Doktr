using AsmResolver.DotNet;
using Doktr.Core.Models.Collections;
using Doktr.Lifters.Common;

namespace Doktr.Lifters.AsmResolver;

public class AsmResolverLifter : IModelLifter
{
    private readonly ModuleDefinition _module;
    private readonly TypeDocumentationCollection _types = new();

    public AsmResolverLifter(string assemblyPath, string xmlPath)
    {
        _module = ModuleDefinition.FromFile(assemblyPath);
    }

    public TypeDocumentationCollection LiftModels()
    {
        return _types;
    }
}