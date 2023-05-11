using System.ComponentModel.DataAnnotations;
using MediatR;
using WASMChat.Shared.Results.Chats;

namespace WASMChat.Shared.Requests.Chats;

public record CreateChatRequest : IRequest<CreateChatResult>
{
    public const int ChatNameMaxLength = 128;
    /// <summary>
    /// The optional owner id. It is filled from http context.
    /// </summary>
    public int OwnerId { get; init; }
    /// <summary>
    /// The required name of the chat.
    /// </summary>
    [MinLength(1), MaxLength(ChatNameMaxLength)]
    public required string ChatName { get; init; }
    /// <summary>
    /// The required list of member chat user ids.
    /// Owner is automatically included.
    /// </summary>
    public required int[] MemberIds { get; init; }
}