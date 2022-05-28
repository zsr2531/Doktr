using MediatR;
using Serilog;

namespace Doktr;

public class LoggerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggerPipelineBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        _logger.Verbose("Processing request {Request}", request);
        var response = await next();
        _logger.Verbose("Got response for {Request}: {Response}", request, response);

        return response;
    }
}