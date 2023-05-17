using Microsoft.JSInterop;
using WASMChat.CommonComponents.JsInterop;
using WASMChat.Pages.Chat.Pages;

namespace WASMChat.Pages.Chat;

public class ChatJsInterop : JsInteropBase
{
    public ChatJsInterop(IJSRuntime jsRuntime) : base(jsRuntime)
    { }
    
    protected override string JsFileName => "chats.js";
    
    /// <summary>
    /// Scrolls down the messages container in <see cref="ChatPage"/>.
    /// </summary>
    public async ValueTask ScrollDown()
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("scrollContainerDown");
    }
    
    /// <summary>
    /// Ensures that user intends to delete a message.
    /// </summary>
    /// <returns><see langword="true"/> if user confirms their intention, otherwise false.</returns>
    public ValueTask<bool> CheckMessageDeletionIntended()
        => Runtime.ConfirmAsync("Вы уверены что хотите удалить сообщение?");
}