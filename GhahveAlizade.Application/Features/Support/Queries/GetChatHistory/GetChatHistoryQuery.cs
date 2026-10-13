using ErrorOr;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetChatHistory;

public record GetChatHistoryQuery(long SessionId, long? AfterMessageId = null)
	: IRequest<ErrorOr<ChatHistoryDto>>;
