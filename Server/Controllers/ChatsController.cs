using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Controllers;

[Authorize]
[ApiController] 
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("users/current")]
    public Task<GetChatUserResult> GetChatUser(
        [FromQuery] GetChatUserRequest request)
        => _mediator.Send(request);

    [HttpGet]
    public Task<GetAllChatsResult> GetChats(
        [FromQuery] GetAllChatsRequest request)
        => _mediator.Send(request);

    [HttpGet("{chatId:int}")]
    public Task<GetChatResult> GetChat(
        [FromRoute] int chatId,
        [FromQuery] GetChatRequest request)
        => _mediator.Send(request with { ChatId = chatId });
    
    [HttpPost]
    public Task<CreateChatResult> CreateChat(
        [FromBody] CreateChatRequest request)
        => _mediator.Send(request);

    [HttpPost("{chatId:int}/messages")]
    public Task<PostChatMessageResult> PostMessage(
        [FromRoute] int chatId,
        [FromBody] PostChatMessageRequest request)
        => _mediator.Send(request with { ChatId = chatId });

    [HttpGet("users")]
    public Task<GetAllUsersResult> GetAllUsers(
        [FromQuery] GetAllUsersRequest request)
        => _mediator.Send(request);
}