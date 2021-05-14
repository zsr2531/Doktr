using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Serilog;

namespace Doktr.Services
{
    public class GraphBuilderService : IGraphBuilderService
    {
        private readonly IAssemblyRepositoryService _repository;
        private readonly ILogger _logger;

        public GraphBuilderService(IAssemblyRepositoryService repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public DependencyGraph BuildGraph()
        {
            var mapping = ImmutableDictionary.CreateBuilder<IFullNameProvider, DependencyNode>();
            var roots = ImmutableArray.CreateBuilder<DependencyNode>();
            
            foreach (var assembly in _repository.LoadedAssemblies)
            {
                _logger.Verbose("Processing '{Assembly}'", assembly.FullName);
                var node = new DependencyNode(assembly);
                roots.Add(node);
                mapping.Add(assembly, node);
            }

            return new DependencyGraph(mapping.ToImmutable(), roots.ToImmutable());
        }
    }
}