using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WASMChat.Server.Data;
using WASMChat.Server.Models;
using WASMChat.Shared.Messages;

namespace WASMChat.Server.Controllers;

[Authorize]
[ApiController] 
[Route("[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ApplicationDbContext _ctx;

    public ChatsController(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(
        [FromBody] string userName)
    {
        string? userId = User.GetUserId();
        var appUser = await _ctx.Users
            .Include(x => x.ChatUser)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (appUser is null) return BadRequest("Бля ты кто");
        if (appUser.ChatUser is not null) return BadRequest("Ты еблан?");
        
        appUser.ChatUser = new ChatUser()
        {
            Name = userName
        };

        await _ctx.SaveChangesAsync();
        return Ok(appUser.ChatUser);
    }
}