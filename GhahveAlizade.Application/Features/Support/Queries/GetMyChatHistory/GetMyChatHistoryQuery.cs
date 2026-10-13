using ErrorOr;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetMyChatHistory;

/// <summary>پیام‌های چت کاربر از بعد از afterMessageId (برای polling). afterMessageId=null ⇒ همه.</summary>
public record GetMyChatHistoryQuery(long UserId, long SessionId, long? AfterMessageId = null)
	: IRequest<ErrorOr<ChatHistoryDto>>;
