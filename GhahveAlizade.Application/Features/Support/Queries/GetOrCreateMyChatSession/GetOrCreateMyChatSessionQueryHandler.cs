using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Support.Dtos;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetOrCreateMyChatSession;

public class GetOrCreateMyChatSessionQueryHandler
	: IRequestHandler<GetOrCreateMyChatSessionQuery, ErrorOr<ChatSessionDto>>
{
	private readonly IAppDbContext _context;

	public GetOrCreateMyChatSessionQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ChatSessionDto>> Handle(GetOrCreateMyChatSessionQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var session = await _context.SupportChatSessions
			.Where(s => !s.IsDeleted && s.UserId == userIdStr
				&& s.Status != ChatSessionStatus.Closed)
			.OrderByDescending(s => s.CreatedAtUtc)
			.FirstOrDefaultAsync(ct);

		if (session is null)
		{
			var customer = await _context.Customers
				.AsNoTracking()
				.Where(c => c.UserId == userIdStr && !c.IsDeleted)
				.Select(c => (long?)c.Id)
				.FirstOrDefaultAsync(ct);

			session = new SupportChatSession
			{
				UserId = userIdStr,
				CustomerId = customer,
				Status = ChatSessionStatus.Waiting
			};
			_context.SupportChatSessions.Add(session);
			await _context.SaveChangesAsync(ct);
		}

		var unread = await _context.ChatMessages
			.CountAsync(m => m.SessionId == session.Id && m.IsFromAdmin
				&& m.ReadAtUtc == null && !m.IsDeleted, ct);

		return new ChatSessionDto
		{
			Id = session.Id,
			Status = session.Status,
			Subject = session.Subject,
			CreatedAtUtc = session.CreatedAtUtc,
			UnreadCount = unread
		};
	}
}
