﻿using WASMChat.Shared.Models.Chats;

namespace WASMChat.Shared.Results.Chats;

public record PostChatMessageResult
{
    public required ChatMessageModel Message { get; init; }
}