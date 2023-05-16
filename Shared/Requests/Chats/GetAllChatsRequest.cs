﻿using System.Security.Claims;
using System.Text.Json.Serialization;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record GetAllChatsRequest : IRequest<GetAllChatsResult>
{
    public ClaimsPrincipal User { get; init; } = null!;
    public required int Page { get; init; } = 0;
}