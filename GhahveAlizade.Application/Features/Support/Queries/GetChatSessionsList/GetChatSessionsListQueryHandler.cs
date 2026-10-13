using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Support.Dtos;
using GhahveAlizade.Domain.Support;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetChatSessionsList;

public class GetChatSessionsListQueryHandler
	: IRequestHandler<GetChatSessionsListQuery, ErrorOr<List<ChatSessionDto>>>
{
	private readonly IAppDbContext _context;

	public GetChatSessionsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<ChatSessionDto>>> Handle(GetChatSessionsListQuery request, CancellationToken ct)
	{
		var query = _context.SupportChatSessions
			.AsNoTracking()
			.Include(s => s.Customer)
			.AsQueryable();

		if (request.Status.HasValue)
			query = query.Where(s => s.Status == request.Status.Value);

		var sessions = await query
			.OrderByDescending(s => s.UpdatedAtUtc ?? s.CreatedAtUtc)
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.Select(s => new ChatSessionDto
			{
				Id = s.Id,
				Status = s.Status,
				Subject = s.Subject,
				CustomerName = s.Customer != null
					? (s.Customer.FirstName + " " + s.Customer.LastName).Trim()
					: s.GuestName,
				GuestPhoneNumber = s.GuestPhoneNumber,
				CreatedAtUtc = s.CreatedAtUtc,
				UnreadCount = _context.ChatMessages
					.Count(m => m.SessionId == s.Id && !m.IsFromAdmin && m.ReadAtUtc == null && !m.IsDeleted)
			})
			.ToListAsync(ct);

		return sessions;
	}
}
