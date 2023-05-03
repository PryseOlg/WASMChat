using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Server.Data.Repositories;
using WASMChat.Server.Models;
using WASMChat.Server.Services;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Controllers;

[Authorize]
[ApiController] 
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ChatRepository _chatRepository;
    private readonly ChatUserService _chatUserService;
    private readonly ChatUserRepository _chatUserRepository;
    private readonly MessageRepository _messageRepository;


    public ChatsController(ChatRepository chatRepository, MessageRepository messageRepository, ChatUserService chatUserService, ChatUserRepository chatUserRepository)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _chatUserService = chatUserService;
        _chatUserRepository = chatUserRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetChats(
        int page = 0)
    {
        var appUserId = User.GetAppUserId();
        if (appUserId is null) return BadRequest(appUserId);
        
        var chatUser = await _chatUserService.GetOrRegister(appUserId);
        var chats = await _chatUserRepository.GetChatsOfUser(chatUser.Id);
        return Ok(chats);
    }



    [HttpPost]
    public async Task<IActionResult> CreateChat(
        [FromQuery] string chatName,
        [FromBody] string[] memberIds)
    {
        var appUserId = User.GetAppUserId();
        if (appUserId is null) return BadRequest(appUserId);
        
        var chatUser = await _chatUserService.GetOrRegister(appUserId);

        var chatMembers = new List<ChatUser> { chatUser};

        foreach (var memberId in memberIds)
        {
            var member = await _chatUserRepository.GetByAppUserId(memberId);
            if (member is not null)
            {
                chatMembers.Add(member);
            }
        }
        
        var newChat = new Chat()
        {
            Name = chatName,
            ChatUsers = chatMembers
        };

        await _chatRepository.SaveChat(newChat);
        newChat.ChatUsers.Clear();
        return Ok(newChat);
    }
    
    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetChat(
        [FromRoute] int chatId)
    {
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        if (chat is null) return NotFound();
        foreach (var user in chat.ChatUsers)
        {
            user.Chats.Clear();
        }
        return Ok(chat);
    }

    [HttpPost("{chatId}/messages")]
    public async Task<IActionResult> AddedMessageToChat(
        [FromBody] string messageText,
        [FromRoute] int chatId)
    {
        var appUserId = User.GetAppUserId();
        if (appUserId is null) return BadRequest(appUserId);
        
        var chatUser = await _chatUserService.GetOrRegister(appUserId);
        
        var chat = await _chatRepository.GetChatByIdAsync(chatId);
        if (chat is null) return BadRequest(chat);
        if (chat.ChatUsers.Contains(chatUser) is false)
            return BadRequest(chat);
        var message = new Message()
        {
            MessageText = messageText,
            AuthorId = chatUser.Id,
            Chat = chat,
            DateTimeSent = DateTime.UtcNow
        };
        await _messageRepository.AddMessage(message);
        return Ok(message);

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