using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WASMChat.Shared.Requests.Chats;

namespace WASMChat.Server.Swagger;

public class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo?.Module.Assembly != typeof(GetCurrentChatUserRequest).Assembly) return;
        if (context.MemberInfo is not PropertyInfo prop) return;
        if (prop.GetCustomAttribute<RequiredMemberAttribute>() is not null) return;

        schema.Properties.Remove(prop.Name);
    }
}