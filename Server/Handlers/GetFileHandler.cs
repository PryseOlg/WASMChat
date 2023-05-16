using MediatR;
using WASMChat.Data.Entities;
using WASMChat.Server.Services;
using WASMChat.Shared.Requests;
using WASMChat.Shared.Results;

namespace WASMChat.Server.Handlers;

public class GetFileHandler : 
    IRequestHandler<GetFileByIdRequest, GetFileResult>,
    IRequestHandler<GetFileByNameRequest, GetFileResult>
{
    private readonly DatabaseFileService _databaseFileService;

    public GetFileHandler(
        DatabaseFileService databaseFileService)
    {
        _databaseFileService = databaseFileService;
    }

    public async Task<GetFileResult> Handle(GetFileByIdRequest request, CancellationToken cancellationToken)
    {
        DatabaseFile file = await _databaseFileService.GetFileByIdAsync(request.Id);
        var result = new GetFileResult
        {
            Content = file.Content,
            MimeType = file.MimeType,
        };

        return result;
    }

    public async Task<GetFileResult> Handle(GetFileByNameRequest request, CancellationToken cancellationToken)
    {
        DatabaseFile file = await _databaseFileService.GetFileByNameAsync(request.Name);
        var result = new GetFileResult
        {
            Content = file.Content,
            MimeType = file.MimeType,
        };

        return result;
    }
}