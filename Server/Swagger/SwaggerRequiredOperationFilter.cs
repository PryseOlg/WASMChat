using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WASMChat.Shared.Requests.Abstractions;

namespace WASMChat.Server.Swagger;

public class SwaggerRequiredOperationFilter : IOperationFilter
{
    private const string UserPropName = nameof(IUserRequest.User);
    private string[] _requiredPropNames = Array.Empty<string>();

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (ContextIsWrong(context)) return;
        ParameterInfo? request = context.MethodInfo
            .GetParameters()
            .SingleOrDefault();
        
        if (request is null) return;

        _requiredPropNames = request.ParameterType.GetProperties()
            .Where(p => p.GetCustomAttribute<RequiredMemberAttribute>() is not null)
            .Select(p => p.Name)
            .ToArray();
        
        operation.Parameters = operation.Parameters
            .Where(ShouldBeDisplayed)
            .ToList();
    }

    private bool ShouldBeDisplayed(OpenApiParameter param)
        => _requiredPropNames.Any(p => p.Equals(param.Name, StringComparison.InvariantCultureIgnoreCase)) &&
           param.Name.Equals(UserPropName, StringComparison.InvariantCultureIgnoreCase) is false;

    private static bool ContextIsWrong(OperationFilterContext ctx)
        => ctx.MethodInfo.Module.Assembly.FullName?.StartsWith("WasmChat", StringComparison.InvariantCultureIgnoreCase)
            is not true;
}