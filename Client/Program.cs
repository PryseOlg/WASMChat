using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WASMChat.Client;
using WASMChat.Pages.Chat;
using WASMChat.Pages.Chat.Services;
using Scrutor;
using WASMChat.CommonComponents.JsInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WASMChat.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WASMChat.ServerAPI"));

builder.Services.AddApiAuthorization();

builder.Services.Scan(scan =>
{
    scan.FromAssemblyDependencies(typeof(Program).Assembly)
        .AddClasses(classes => classes.AssignableTo<JsInteropBase>())
        .AsSelf();
});

builder.Services.AddScoped<ChatHubClient>();
builder.Services.AddScoped<CurrentUserAccessor>();

await builder.Build().RunAsync();
