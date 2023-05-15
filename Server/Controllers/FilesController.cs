using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WASMChat.Data.Repositories;
using WASMChat.Server.Exceptions;

namespace WASMChat.Server.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class FilesController : Controller
{
    private readonly DatabaseFileRepository _dbFileRepo;

    public FilesController(
        DatabaseFileRepository dbFileRepo)
    {
        _dbFileRepo = dbFileRepo;
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetFile(
        [FromRoute] string fileName)
    {
        var file = await _dbFileRepo.GetByNameAsync(fileName);
        NotFoundException.ThrowIfNull(file);

        MemoryStream ms = new(file.Content);
        ms.Position = 0;
        return File(ms, MediaTypeNames.Image.Jpeg);
    }
}