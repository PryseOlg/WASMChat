using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace WASMChat.CommonComponents.ApiClients;

public abstract class HubClientBase
{
    private readonly IAccessTokenProvider _accessAccessTokenProvider;
    protected abstract string HubRelativeUrl { get; }
    public HubConnection Connection { get; }

    protected HubClientBase(
        IAccessTokenProvider accessTokenProvider, 
        NavigationManager navigationManager)
    {
        _accessAccessTokenProvider = accessTokenProvider;
        Uri hubUri = navigationManager.ToAbsoluteUri(HubRelativeUrl);
        Connection = new HubConnectionBuilder()
            .WithUrl(hubUri, options =>
            {
                options.AccessTokenProvider = RetrieveToken;
            })
            .Build();
    }
    
    public Task StartAsync() => Connection.StartAsync();

    private async Task<string?> RetrieveToken()
    {
        AccessTokenResult? tokenResult = await _accessAccessTokenProvider.RequestAccessToken();
        tokenResult.TryGetToken(out AccessToken? token);
        return token?.Value;
    }
}