﻿using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Requests.Chats.Messages;
using WASMChat.Shared.Results.Chats;
using WASMChat.Shared.Results.Chats.Messages;

namespace WASMChat.Server.Controllers;

[Authorize]
[ApiController] 
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Gets caller's chat user or creates one.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("users/current")]
    public Task<GetCurrentChatUserResult> GetChatUser(
        [FromQuery] GetCurrentChatUserRequest request)
        => _mediator.Send(request with { User = User });
    
    /// <summary>
    /// Gets all chats of current chat user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetAllChatsResult> GetChats(
        [FromQuery] GetAllChatsRequest request)
        => _mediator.Send(request with { User = User });
    
    /// <summary>
    /// Gets chat with specified id.
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("{chatId:int}")]
    public Task<GetChatResult> GetChat(
        [FromRoute] int chatId,
        [FromQuery] GetChatRequest request)
        => _mediator.Send(request with { ChatId = chatId, User = User });
    
    /// <summary>
    /// Creates chat with specified name and users.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<CreateChatResult> CreateChat(
        [FromBody] CreateChatRequest request)
        => _mediator.Send(request with { User = User });

    /// <summary>
    /// Gets all chat users.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("users")]
    public Task<GetAllUsersResult> GetAllUsers(
        [FromQuery] GetAllUsersRequest request)
        => _mediator.Send(request);
}