using System.Reflection;
using Microsoft.JSInterop;

namespace WASMChat.CommonComponents.JsInterop;

public abstract class JsInteropBase : IAsyncDisposable
{
    /// <summary>
    /// The name of the .js file as it is called in local wwwroot.
    /// </summary>
    protected abstract string JsFileName { get; }
    
    /// <summary>
    /// The underlying <see cref="IJSRuntime"/> for using
    /// default js functions.
    /// </summary>
    protected IJSRuntime Runtime { get; }

    /// <summary>
    /// Full path to .js file.
    /// </summary>
    private string JsFileFullPath => $"./_content/{SubfolderName}/{JsFileName}";
    
    /// <summary>
    /// When in derived class, returns a subfolder name for current project.
    /// </summary>
    private string? SubfolderName => GetType().Assembly.GetName().Name;
    
    /// <summary>
    /// The underlying <see cref="IJSRuntime"/> object, wrapped for lazy evaluation.
    /// </summary>
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    
    /// <summary>
    /// Gets the imported javascript module.
    /// </summary>
    /// <returns></returns>
    protected Task<IJSObjectReference> GetModuleAsync() => _moduleTask.Value;

    protected JsInteropBase(IJSRuntime jsRuntime)
    {
        Runtime = jsRuntime;
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() => 
            Runtime.InvokeAsync<IJSObjectReference>("import", JsFileFullPath).AsTask());
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            IJSObjectReference module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}