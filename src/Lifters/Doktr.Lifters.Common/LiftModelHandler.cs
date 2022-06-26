using System.Diagnostics.CodeAnalysis;
using Doktr.Core;
using Doktr.Core.Models.Collections;
using MediatR;

namespace Doktr.Lifters.Common;

[ExcludeFromCodeCoverage]
public class LiftModelHandler : IRequestHandler<LiftModel, AssemblyTypesMap>
{
    private readonly IModelLifter _lifter;
    private readonly DoktrConfiguration _configuration;

    public LiftModelHandler(IModelLifter lifter, DoktrConfiguration configuration)
    {
        _lifter = lifter;
        _configuration = configuration;
    }

    public Task<AssemblyTypesMap> Handle(LiftModel request, CancellationToken cancellationToken)
    {
        var types = _lifter.LiftModels(_configuration.InputFiles);
        return Task.FromResult(types);
    }
}