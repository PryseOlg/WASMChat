using Microsoft.AspNetCore.Mvc;
using WASMChat.Shared.Results;

namespace WASMChat.Server.Extensions;

public static class ResultsExtensions
{
    public static async ValueTask<FileStreamResult> AwaitFileStreamResult(this Task<GetFileResult> resultTask)
    {
        GetFileResult result = await resultTask;
        return new FileStreamResult(new MemoryStream(result.Content), result.MimeType);
    }
}