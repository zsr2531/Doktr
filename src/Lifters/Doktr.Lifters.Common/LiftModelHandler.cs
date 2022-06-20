using System.Diagnostics.CodeAnalysis;
using Doktr.Core;
using Doktr.Core.Models.Collections;
using MediatR;

namespace Doktr.Lifters.Common;

[ExcludeFromCodeCoverage]
public class LiftModelHandler : IRequestHandler<LiftModel, AssemblyTypesMap>
{
    private readonly Func<DoktrTarget, IModelLifter> _lifterFactory;
    private readonly DoktrConfiguration _configuration;

    public LiftModelHandler(Func<DoktrTarget, IModelLifter> lifterFactory, DoktrConfiguration configuration)
    {
        _lifterFactory = lifterFactory;
        _configuration = configuration;
    }

    public Task<AssemblyTypesMap> Handle(LiftModel request, CancellationToken cancellationToken)
    {
        var types = new AssemblyTypesMap();

        foreach (var target in _configuration.InputFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lifter = _lifterFactory(target);
            (string fullName, var models) = lifter.LiftModels();
            types[fullName] = models;
        }

        return Task.FromResult(types);
    }
}