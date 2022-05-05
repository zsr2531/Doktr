using Doktr.Core;
using Doktr.Core.Models.Collections;
using MediatR;

namespace Doktr.Lifters.Common;

public class LiftModelHandler : IRequestHandler<LiftModel, TypeDocumentationCollection>
{
    private readonly Func<string, string, IModelLifter> _lifterFactory;
    private readonly DoktrConfiguration _configuration;

    public LiftModelHandler(Func<string, string, IModelLifter> lifterFactory, DoktrConfiguration configuration)
    {
        _lifterFactory = lifterFactory;
        _configuration = configuration;
    }

    public Task<TypeDocumentationCollection> Handle(LiftModel request, CancellationToken cancellationToken)
    {
        var types = new TypeDocumentationCollection();

        foreach ((string assemblyPath, string xmlPath) in _configuration.InputFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var lifter = _lifterFactory(assemblyPath, xmlPath);
            var models = lifter.LiftModels();
            foreach (var model in models)
                types.Add(model);
        }

        return Task.FromResult(types);
    }
}