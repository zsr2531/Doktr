using AsmResolver.DotNet;

namespace Doktr.Services;

public interface IMetadataResolutionService
{
    TypeDefinition? ResolveType(ITypeDefOrRef? type);

    MethodDefinition? ResolveMethod(IMethodDefOrRef? method);

    IMetadataMember? ResolveMember(IMemberDescriptor? descriptor);
}