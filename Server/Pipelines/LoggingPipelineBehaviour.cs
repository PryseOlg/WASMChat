using MediatR;

namespace WASMChat.Server.Pipelines;

public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull
{
    private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;

    public LoggingPipelineBehaviour(
        ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestTypeName = typeof(TRequest).FullName;
        var responseTypeName = typeof(TResponse).FullName;
        _logger.LogInformation("Processing request of type {RequestType} with value {Request}," +
                               "expecting response type {ResponseType}",
            requestTypeName, request, responseTypeName);

        TResponse response = await next();

        _logger.LogInformation("Processed request of type {RequestType} with value {Request}, " +
                               "responding with response type {ResponseType} with value {Response}",
            requestTypeName, request, responseTypeName, response);

        return response;
    }
}