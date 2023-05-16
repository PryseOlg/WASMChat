using MediatR;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Shared.Requests;

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
        [FromQuery] GetFileRequest request)
    {
        var result = await _mediator.Send(request with { Name = fileName });
        return new FileStreamResult(new MemoryStream(result.Content), result.MimeType);
    }
    

    public async Task<FileStreamResult> GetFileById(
        [FromQuery] GetFileRequest request)
    {
        var result = await _mediator.Send(request);
        return new FileStreamResult(new MemoryStream(result.Content), result.MimeType);
    }
}