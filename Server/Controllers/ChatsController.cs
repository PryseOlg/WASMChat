using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services.Chats;
using WASMChat.Shared.Requests.Chats;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Controllers;

[Authorize]
[ApiController] 
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ChatMessageService _chatMessageService;
    private readonly ChatMessageModelMapper _chatMessageModelMapper;
    
    private readonly ChatService _chatService;
    private readonly ChatModelMapper _chatModelMapper;

    private readonly ChatUserService _chatUserService;
    private readonly ChatUserModelMapper _chatUserModelMapper;

    public ChatsController(
        ChatMessageService chatMessageService, 
        ChatMessageModelMapper chatMessageModelMapper, 
        ChatService chatService, 
        ChatModelMapper chatModelMapper,
        ChatUserService chatUserService, 
        ChatUserModelMapper chatUserModelMapper)
    {
        _chatMessageService = chatMessageService;
        _chatMessageModelMapper = chatMessageModelMapper;
        _chatService = chatService;
        _chatModelMapper = chatModelMapper;
        _chatUserService = chatUserService;
        _chatUserModelMapper = chatUserModelMapper;
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> GetChatUser(
        [FromQuery] GetChatUserRequest request)
    {
        var user = await _chatUserService.GetUserAsync(request, HttpContext);

        var result = new GetChatUserResult
        {
            User = _chatUserModelMapper.Create(user)
        };
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetChats(
        [FromQuery] GetAllChatsRequest request)
    {
        var chats = await _chatService.GetAllChatsAsync(request, HttpContext);

        var result = new GetAllChatsResult
        {
            Chats = chats.Select(_chatModelMapper.Create).ToArray()
        };
        return Ok(result);
    }

    [HttpGet("{chatId:int}")]
    public async Task<IActionResult> GetChat(
        [FromRoute] int chatId,
        [FromQuery] GetChatRequest request)
    {
        request.ChatId = chatId;
        var chat = await _chatService.GetChatAsync(request, HttpContext);

        var result = new GetChatResult
        {
            Chat = _chatModelMapper.Create(chat)
        };
        return Ok(result);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateChat(
        [FromBody] CreateChatRequest request)
    {
        var chat = await _chatService.CreateChatAsync(request, HttpContext);

        var result = new CreateChatResult()
        {
            Chat = _chatModelMapper.Create(chat)
        };
        return Ok(result);
    }

    [HttpPost("{chatId:int}/messages")]
    public async Task<IActionResult> PostMessage(
        [FromRoute] int chatId,
        [FromBody] PostChatMessageRequest request)
    {
        request.ChatId = chatId;

        ChatMessage message = await _chatMessageService.SendMessageAsync(request, HttpContext);
        
        var result = new PostChatMessageResult()
        {
            Message = _chatMessageModelMapper.Create(message)
        };
        return Ok(result);
    }
}