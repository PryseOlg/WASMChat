using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WASMChat.Server.Swagger;

public class SwaggerRequiredOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.Module.Assembly.FullName?.StartsWith("WasmChat") is not true) return;
        var request = context.MethodInfo
            .GetParameters()
            .SingleOrDefault(p =>
                p.GetCustomAttribute<FromQueryAttribute>() is not null ||
                p.GetCustomAttribute<FromBodyAttribute>() is not null);

        if (request is null) return;

        var requiredPropNames = request.ParameterType.GetProperties()
            .Where(p => p.GetCustomAttribute<RequiredMemberAttribute>() is not null)
            .Select(p => p.Name)
            .ToArray();

        operation.Parameters = operation.Parameters
            .Where(p => requiredPropNames.Contains(p.Name))
            .ToList();
    }
}