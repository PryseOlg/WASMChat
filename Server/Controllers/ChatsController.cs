using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WASMChat.Server.Data;
using WASMChat.Server.Data.Repositories;
using WASMChat.Server.Models;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Controllers;

[Authorize]
[ApiController] 
[Route("[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ApplicationUserRepository _appUserRepository;
    private readonly ChatUserRepository _chatUserRepository;
    private readonly ChatRepository _chatRepository;
    

    public ChatsController(ApplicationUserRepository appUserRepository, ChatUserRepository chatUserRepository, ChatRepository chatRepository)
    {
        _appUserRepository = appUserRepository;
        _chatUserRepository = chatUserRepository;
        _chatRepository = chatRepository;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(
        [FromBody] string userName)
    {
        var appUser = await _appUserRepository.GetByClaimsPrincipal(User);
        
        if (appUser is null) return BadRequest("Бля ты кто");
        if (appUser.ChatUser is not null) return BadRequest("Ты еблан?");
        
        appUser.ChatUser = new ChatUser()
        {
            Name = userName
        };

        try
        {
            await _appUserRepository.CommitAsync();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        return Ok(appUser.ChatUser);
    }

    [HttpGet]
    public async Task<IActionResult> GetChats(
        int page = 0)
    {
        var chatUser = await _chatUserRepository.GetByClaimsPrincipal(User);
        if (chatUser is null) return BadRequest("Ты даже не гражданин");
        var chats = await _chatUserRepository.GetChatsOfUser(chatUser.Id);
        return Ok(chats);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChat(
        [FromBody] string chatName)
    {
        var chatUser = await _chatUserRepository.GetByClaimsPrincipal(User);
        if (chatUser is null) return BadRequest("Ты даже не гражданин, обосанный");

        var newChat = new Chat()
        {
            Name = chatName,
            ChatUsers = new List<ChatUser> { chatUser }
        };

        await _chatRepository.SaveChat(newChat);
        newChat.ChatUsers.Clear();
        return Ok(newChat);
    }
}