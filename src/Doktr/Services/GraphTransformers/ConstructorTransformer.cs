using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Doktr.Dependencies;
using Serilog;

namespace Doktr.Services.GraphTransformers;

public class ConstructorTransformer : IDependencyGraphTransformer
{
    private readonly IMetadataResolutionService _resolution;
    private readonly ILogger _logger;

    public ConstructorTransformer(IMetadataResolutionService resolution, ILogger logger)
    {
        _resolution = resolution;
        _logger = logger;
    }

    public string Name => nameof(ConstructorTransformer);

    public void VisitNode(DependencyNode node, GraphBuilderContext context)
    {
        if (node.MetadataMember is not MethodDefinition { IsConstructor: true, IsStatic: false } ctor)
            return;

        var body = ctor.CilMethodBody;
        if (body == null)
        {
            _logger.Warning("Failed to resolve base constructor dependency of '{Constructor}'", ctor);
            return;
        }

        foreach (var instruction in body.Instructions)
        {
            if (instruction is not { OpCode: { Code: CilCode.Call }, Operand: IMethodDefOrRef target })
                continue;
            if (_resolution.ResolveMethod(target) is not { IsConstructor: true } resolved)
                continue;

            context.AddDependency(ctor, resolved);
            _logger.Debug("Base constructor of '{Ctor1}' is '{Ctor2}'", ctor, resolved);
            return;
        }
    }
}