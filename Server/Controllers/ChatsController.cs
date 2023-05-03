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
    private readonly ApplicationUserRepository _userRepo;

    public ChatsController(ApplicationUserRepository userRepo, ChatUserRepository chatUserRepository)
    {
        _userRepo = userRepo;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(
        [FromBody] string userName)
    {
        var appUser = await _userRepo.GetByClaimsPrincipal(User);
        
        if (appUser is null) return BadRequest("Бля ты кто");
        if (appUser.ChatUser is not null) return BadRequest("Ты еблан?");
        
        appUser.ChatUser = new ChatUser()
        {
            Name = userName
        };

        try
        {
            await _userRepo.CommitAsync();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        return Ok(appUser.ChatUser);
    }
}