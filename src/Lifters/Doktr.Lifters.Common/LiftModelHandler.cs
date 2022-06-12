using Doktr.Core;
using Doktr.Core.Models.Collections;
using MediatR;

namespace Doktr.Lifters.Common;

public class LiftModelHandler : IRequestHandler<LiftModel, AssemblyTypesMap>
{
    private readonly Func<string, string, IModelLifter> _lifterFactory;
    private readonly DoktrConfiguration _configuration;

    public LiftModelHandler(Func<string, string, IModelLifter> lifterFactory, DoktrConfiguration configuration)
    {
        _lifterFactory = lifterFactory;
        _configuration = configuration;
    }

    public Task<AssemblyTypesMap> Handle(LiftModel request, CancellationToken cancellationToken)
    {
        var types = new AssemblyTypesMap();

        foreach ((string assemblyPath, string xmlPath) in _configuration.InputFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lifter = _lifterFactory(assemblyPath, xmlPath);
            (string fullName, var models) = lifter.LiftModels();
            types[fullName] = models;
        }

        return Task.FromResult(types);
    }
}