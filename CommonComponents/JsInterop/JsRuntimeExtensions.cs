using Microsoft.JSInterop;

namespace WASMChat.CommonComponents.JsInterop;

public static class JsRuntimeExtensions
{
    public static ValueTask AlertAsync(this IJSRuntime runtime, string message)
        => runtime.InvokeVoidAsync("alert", message);

    public static ValueTask<bool> ConfirmAsync(this IJSRuntime runtime, string message)
        => runtime.InvokeAsync<bool>("confirm", message);
    
    public static ValueTask<string> PromptAsync(this IJSRuntime runtime, string message)
        => runtime.InvokeAsync<string>("prompt", message);
}