using ErrorOr;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Support.Queries.GetOrCreateMyChatSession;

/// <summary>
/// جلسه چت باز کاربر را برمی‌گرداند؛ اگر باز نداشته باشد، یکی می‌سازد.
/// </summary>
public record GetOrCreateMyChatSessionQuery(long UserId) : IRequest<ErrorOr<ChatSessionDto>>;
