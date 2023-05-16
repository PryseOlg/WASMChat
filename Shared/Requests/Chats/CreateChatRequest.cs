using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using WASMChat.Shared.Constants;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record CreateChatRequest : IRequest<CreateChatResult>
{
    public ClaimsPrincipal? User { get; init; }
    [MinLength(1), MaxLength(Database.ChatNameMaxLength)]
    public required string ChatName { get; init; }
    public required int[] MemberIds { get; init; }
}