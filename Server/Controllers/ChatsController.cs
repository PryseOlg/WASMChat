using System.Net.Mime;
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
    public Task<GetChatUserResult> GetChatUser(
        [FromQuery] GetChatUserRequest request)
        => _mediator.Send(request);
    
    /// <summary>
    /// Gets all chats of current chat user.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetAllChatsResult> GetChats(
        [FromQuery] GetAllChatsRequest request)
        => _mediator.Send(request);
    
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
        => _mediator.Send(request with { ChatId = chatId });
    
    /// <summary>
    /// Creates chat with specified name and users.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<CreateChatResult> CreateChat(
        [FromBody] CreateChatRequest request)
        => _mediator.Send(request);
    
    /// <summary>
    /// Sends message to specified chat.
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{chatId:int}/messages")]
    public Task<PostChatMessageResult> PostMessage(
        [FromRoute] int chatId,
        [FromBody] PostChatMessageRequest request)
        => _mediator.Send(request with { ChatId = chatId });
    
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