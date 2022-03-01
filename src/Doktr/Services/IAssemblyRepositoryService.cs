using System.Collections.Generic;
using AsmResolver.DotNet;

namespace Doktr.Services;

public interface IAssemblyRepositoryService
{
    IReadOnlyList<AssemblyDefinition> LoadedAssemblies { get; }

    bool LoadAssembly(string path);
}