using AsmResolver.DotNet;
using Serilog;

namespace Doktr.Services;

public class MetadataResolutionService : IMetadataResolutionService
{
    private readonly ILogger _logger;

    public MetadataResolutionService(ILogger logger)
    {
        _logger = logger;
    }

    public TypeDefinition? ResolveType(ITypeDefOrRef? type)
    {
        var resolved = type?.Resolve();
        if (type is not null && resolved is null)
            _logger.Warning("Failed to resolve type reference {Type}", type);

        return resolved;
    }

    public MethodDefinition? ResolveMethod(IMethodDefOrRef? method)
    {
        var resolved = method?.Resolve();
        if (method is not null && resolved is null)
            _logger.Warning("Failed to resolve method reference {Method}", method);

        return resolved;
    }

    public IMetadataMember? ResolveMember(IMemberDescriptor? descriptor)
    {
        var resolved = descriptor?.Resolve();
        if (descriptor is not null && resolved is null)
            _logger.Warning("Failed to resolve member reference {Member}", descriptor);

        return resolved;
    }
}