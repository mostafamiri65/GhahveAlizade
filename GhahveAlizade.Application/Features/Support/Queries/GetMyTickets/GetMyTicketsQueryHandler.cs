using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Support.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetMyTickets;

public class GetMyTicketsQueryHandler
	: IRequestHandler<GetMyTicketsQuery, ErrorOr<PaginatedList<TicketListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetMyTicketsQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<TicketListItemDto>>> Handle(
		GetMyTicketsQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId.ToString();

		var query = _context.SupportTickets
			.AsNoTracking()
			.Include(t => t.Messages)
			.Where(t => t.UserId == userIdStr)
			.OrderByDescending(t => t.UpdatedAtUtc ?? t.CreatedAtUtc);

		if (request.Status.HasValue)
			query = (IOrderedQueryable<Domain.Support.SupportTicket>)query
				.Where(t => t.Status == request.Status.Value);

		var dtoQuery = query.ProjectToType<TicketListItemDto>()
			// پاسخ خوانده‌نشده ادمین: آخرین پیام از ادمین و ReadAtUtc == null
			.ProjectToType<TicketListItemDto>();

		var projected = query.Select(t => new TicketListItemDto
		{
			Id = t.Id,
			TicketNumber = t.TicketNumber,
			Subject = t.Subject,
			Category = t.Category,
			Status = t.Status,
			Priority = t.Priority,
			MessageCount = t.Messages.Count,
			CreatedAtUtc = t.CreatedAtUtc,
			LastMessageAtUtc = t.Messages.Max(m => (DateTime?)m.CreatedAtUtc),
			HasUnreadAdminReply = t.Messages
				.Where(m => m.IsFromAdmin && m.ReadAtUtc == null)
				.Any()
		});

		return await PaginatedList<TicketListItemDto>.CreateAsync(
			projected, request.PageNumber, request.PageSize, ct);
	}
}
