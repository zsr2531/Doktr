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
        var (signature) = request;
        var decompilationStrategy = CreateDecompilationStrategy();
        signature.AcceptVisitor(decompilationStrategy);

        return Task.FromResult(decompilationStrategy.ToString());
    }

    private ITypeSignatureDecompilationStrategy CreateDecompilationStrategy()
    {
        return _configuration.EnableNrt switch
        {
            true => new NullableTypeSignatureDecompilationStrategy(_configuration),
            false => new NormalTypeSignatureDecompilationStrategy()
        };
    }
}