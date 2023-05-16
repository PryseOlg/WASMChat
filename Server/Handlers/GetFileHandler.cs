using MediatR;
using WASMChat.Data.Entities.Files;
using WASMChat.Server.Services;
using WASMChat.Shared.Requests;
using WASMChat.Shared.Results;

namespace WASMChat.Server.Handlers;

public class GetFileHandler : IRequestHandler<GetFileRequest, GetFileResult>
{
    private readonly DatabaseFileService _databaseFileService;

    public GetFileHandler(
        DatabaseFileService databaseFileService)
    {
        _databaseFileService = databaseFileService;
    }

    public async Task<GetFileResult> Handle(GetFileRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) && request.Id is null)
            throw new ArgumentException("You need to specified either file name or id", nameof(request));

        DatabaseFile file = string.IsNullOrWhiteSpace(request.Name)
            ? await _databaseFileService.GetFileByIdAsync(request.Id!.Value)
            : await _databaseFileService.GetFileByNameAsync(request.Name);

        var result = new GetFileResult
        {
            Content = file.Content,
            MimeType = file.MimeType,
        };

        return result;
    }
}