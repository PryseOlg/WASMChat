using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace WASMChat.CommonComponents.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResult> SafeFetch<TResult>(this HttpClient client, string url)
    {
        try
        {
            var result = await client.GetFromJsonAsync<TResult>(url);
            ArgumentNullException.ThrowIfNull(result);
            return result;
        }
        catch (AccessTokenNotAvailableException e)
        {
            e.Redirect();
            
            await Task.Delay(-1);
            throw;
        }
    }
}