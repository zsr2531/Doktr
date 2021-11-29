using System.Collections.Generic;
using System.IO;
using AsmResolver.DotNet;
using Serilog;

namespace Doktr.Services;

public class AssemblyRepositoryService : IAssemblyRepositoryService
{
    private readonly List<AssemblyDefinition> _assemblies = new();
    private readonly ILogger _logger;

    public AssemblyRepositoryService(ILogger logger)
    {
        _logger = logger;
    }

    public IReadOnlyList<AssemblyDefinition> LoadedAssemblies => _assemblies;

    public bool LoadAssembly(string path)
    {
        _logger.Verbose("Loading assembly from '{Path}'", path);

        try
        {
            var assembly = AssemblyDefinition.FromFile(path);
            _assemblies.Add(assembly);
            _logger.Debug("'{Assembly}' successfully loaded and added to repository", assembly.FullName);

            return true;
        }
        catch (IOException ex)
        {
            _logger.Error(ex, "An IO error occured");
            return false;
        }
    }
}