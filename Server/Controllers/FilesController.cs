using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Server.Extensions;
using WASMChat.Shared.Requests;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class FilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilesController(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{name:file}")]
    public async Task<FileStreamResult> GetFileByName(
        [FromRoute] GetFileByNameRequest request)
        => await _mediator.Send(request).AwaitFileStreamResult();

    [HttpGet]
    public async Task<FileStreamResult> GetFileById(
        [FromQuery] GetFileByIdRequest request)
        => await _mediator.Send(request).AwaitFileStreamResult();
    
    [HttpPost]
    [Authorize]
    public async Task<PostFileResult> PostFile(
        [FromBody] PostFileRequest request)
        => await _mediator.Send(request);
}