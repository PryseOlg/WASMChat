using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Data.Entities.Chats;
using WASMChat.Server.Extensions;
using WASMChat.Server.Mappers.Chats;
using WASMChat.Server.Services;
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
    private readonly ChatModelMapper _chatModelMapper;
    private readonly ChatService _chatService;

    public ChatsController(ChatMessageService chatMessageService, 
        ChatMessageModelMapper chatMessageModelMapper, 
        ChatModelMapper chatModelMapper, 
        ChatService chatService)
    {
        _chatMessageService = chatMessageService;
        _chatMessageModelMapper = chatMessageModelMapper;
        _chatModelMapper = chatModelMapper;
        _chatService = chatService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetChats(
        [FromBody] GetAllChatsRequest request)
    {
        var chats = await _chatService.GetAllChatsAsync(request, User);

        var result = new GetAllChatsResult
        {
            Chats = chats.Select(_chatModelMapper.Create).ToArray()
        };
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChat(
        [FromBody] CreateChatRequest request)
    {
        var chat = await _chatService.CreateChatAsync(request, User);

        var result = new CreateChatResult()
        {
            Chat = _chatModelMapper.Create(chat)
        };
        return Ok(result);
    }
    
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetChat(
        [FromRoute] int chatId,
        [FromBody] GetChatRequest request)
    {
        request.ChatId = chatId;
        Chat? chat = await _chatService.GetChatAsync(request, User);
        if (chat is null) return NotFound();

        var result = new GetChatResult
        {
            Chat = _chatModelMapper.Create(chat)
        };

        return Ok(result);
    }

    [HttpPost("{chatId}/messages")]
    public async Task<IActionResult> PostMessage(
        [FromRoute] int chatId,
        [FromBody] PostChatMessageRequest request)
    {
        request.ChatId = chatId;

        ChatMessage message = await _chatMessageService.SendMessageAsync(request);
        
        var result = new PostChatMessageResult()
        {
            Message = _chatMessageModelMapper.Create(message)
        };
        return Ok(result);
    }

    /*[HttpPost("{chatId}/join")]
    public async Task<IActionResult> JoinChat(
        [FromRoute] int chatId)
    {
        var appUserId = User.GetAppUserId();
        if (appUserId is null) return BadRequest(appUserId);
        
        var chatUser = await _chatUserService.GetOrRegister(appUserId);
        
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        if (chat is null) return BadRequest(chat);
        
        chatUser.Chats.Add(chat);
        await _chatUserRepository.Update(chatUser);
        return Ok();
    }*/
    
    


}