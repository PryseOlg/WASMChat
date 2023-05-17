using MediatR;
using WASMChat.Server.Exceptions;
using WASMChat.Shared.Requests.Abstractions;

namespace WASMChat.Server.Pipelines;

public class HttpInjectorPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IUserRequest, new()
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpInjectorPipelineBehaviour(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.User ??= _httpContextAccessor.HttpContext?.User;
        UnauthorizedException.ThrowIfNull(request.User);

        return await next();
    }
}