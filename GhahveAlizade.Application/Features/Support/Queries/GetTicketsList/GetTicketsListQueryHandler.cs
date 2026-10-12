using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Support.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Support.Queries.GetTicketsList;

public class GetTicketsListQueryHandler
	: IRequestHandler<GetTicketsListQuery, ErrorOr<PaginatedList<AdminTicketListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetTicketsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<AdminTicketListItemDto>>> Handle(
		GetTicketsListQuery request, CancellationToken ct)
	{
		var query = _context.SupportTickets
			.AsNoTracking()
			.Include(t => t.Customer)
			.Include(t => t.AssignedTo)
			.Include(t => t.Messages)
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(t =>
				t.TicketNumber.Contains(s) || t.Subject.Contains(s) ||
				(t.Customer != null && t.Customer.PhoneNumber != null && t.Customer.PhoneNumber.Contains(s)) ||
				(t.Customer != null && t.Customer.FirstName != null && t.Customer.FirstName.Contains(s)));
		}

		if (request.Status.HasValue)
			query = query.Where(t => t.Status == request.Status.Value);

		if (request.Priority.HasValue)
			query = query.Where(t => t.Priority == request.Priority.Value);

		var projected = query
			.OrderByDescending(t => t.UpdatedAtUtc ?? t.CreatedAtUtc)
			.Select(t => new AdminTicketListItemDto
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
				CustomerName = t.Customer != null
					? (t.Customer.FirstName + " " + t.Customer.LastName).Trim()
					: t.GuestName,
				CustomerPhone = t.Customer != null ? t.Customer.PhoneNumber : t.GuestPhoneNumber,
				AssignedToUserId = t.AssignedToUserId,
				AssignedToName = t.AssignedTo != null
					? (t.AssignedTo.FirstName + " " + t.AssignedTo.LastName).Trim()
					: null,
				HasUnreadCustomerMessage = t.Messages
					.Where(m => !m.IsFromAdmin && m.ReadAtUtc == null)
					.Any()
			});

		return await PaginatedList<AdminTicketListItemDto>.CreateAsync(
			projected, request.PageNumber, request.PageSize, ct);
	}
}
