using MediatR;
using WASMChat.Data.Entities;
using WASMChat.Server.Services;
using WASMChat.Shared.Requests;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Server.Handlers;

public class PostFileHandler : IRequestHandler<PostFileRequest, PostFileResult>
{
    private readonly DatabaseFileService _databaseFileService;

    public PostFileHandler(
        DatabaseFileService databaseFileService)
    {
        _databaseFileService = databaseFileService;
    }

    public async Task<PostFileResult> Handle(PostFileRequest request, CancellationToken cancellationToken)
    {
        DatabaseFile savedFile = await _databaseFileService.SaveDatabaseFile(
            request.Content, request.MimeType, request.FileName, request.Scope);

        var result = new PostFileResult
        {
            FileName = savedFile.Name,
            FileId = savedFile.Id
        };

        return result;
    }
}