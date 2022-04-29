using Doktr.Core;
using Doktr.Decompiler.Signatures;
using MediatR;

namespace Doktr.Decompiler;

public class DecompileTypeSignatureHandler : IRequestHandler<DecompileTypeSignature, string>
{
    private readonly DoktrConfiguration _configuration;

    public DecompileTypeSignatureHandler(DoktrConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> Handle(DecompileTypeSignature request, CancellationToken cancellationToken)
    {
        var signature = request.Signature;
        var decompilationStrategy = CreateDecompilationStrategy();
        signature.AcceptVisitor(decompilationStrategy);

        return Task.FromResult(decompilationStrategy.ToString());
    }

    private TypeSignatureDecompilationStrategy CreateDecompilationStrategy()
    {
        return _configuration.EnableNrt switch
        {
            true => new NullableTypeSignatureDecompilationStrategy(),
            false => new TypeSignatureDecompilationStrategy()
        };
    }
}