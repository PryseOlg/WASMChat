using MediatR;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Server.Extensions;
using WASMChat.Shared.Requests;
using WASMChat.Shared.Results;
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
    
    [HttpGet("{fileName}")]
    public async Task<FileStreamResult> GetFileByName(
        [FromRoute] string fileName,
        [FromQuery] GetFileByNameRequest request)
        => await _mediator.Send(request with { Name = fileName }).AwaitFileStreamResult();

    [HttpGet]
    public async Task<FileStreamResult> GetFileById(
        [FromQuery] GetFileByIdRequest request)
        => await _mediator.Send(request).AwaitFileStreamResult();
    
    [HttpPost]
    public async Task<PostFileResult> PostFile(
        [FromBody] PostFileRequest request)
        => await _mediator.Send(request);
}